using SpiceDb.Models;

namespace ArchimedesPermissionsTest;

public static class ZedConfig
{
    public static string Prefix = "archtest";
}

public class ZedCustomRole : ResourceReference
{
    private ZedCustomRole(string id) : base($"{ZedConfig.Prefix}/custom_role:{id}")
    {
    }

    public static ZedCustomRole WithId(string id) => new ZedCustomRole(id);

    // Relationships
    public Relationship RelateParent(ZedGroup subject) => new Relationship(this, "parent", subject);
    public Relationship RelateMember(ZedUser subject) => new Relationship(this, "member", subject);
}

public class ZedPlatform : ResourceReference
{
    private ZedPlatform(string id) : base($"{ZedConfig.Prefix}/platform:{id}")
    {
    }

    public static ZedPlatform WithId(string id) => new ZedPlatform(id);

    // Relationships
    public Relationship RelateAdministrator(ZedUser subject) => new Relationship(this, "administrator", subject);
}

public class ZedOrganization : ResourceReference
{
    private ZedOrganization(string id) : base($"{ZedConfig.Prefix}/organization:{id}")
    {
    }

    public static ZedOrganization WithId(string id) => new ZedOrganization(id);

    // Relationships
    public Relationship RelateMember(ZedUser subject) => new Relationship(this, "member", subject);
    public Relationship RelatePlatform(ZedPlatform subject) => new Relationship(this, "platform", subject);
}

public class ZedGroup : ResourceReference
{
    private ZedGroup(string id) : base($"{ZedConfig.Prefix}/group:{id}")
    {
    }

    // Create Group object
    public static ZedGroup WithId(string id) => new ZedGroup(id);

    // Relationships
    /// <summary>
    /// Organization responsible for this group
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateOrganization(ZedOrganization subject) => new Relationship(this, "organization", subject);

    /// <summary>
    /// Allows user to be owner of this group
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateOwner(ZedUser subject) => new Relationship(this, "owner", subject);
    public Relationship RelateManager(ZedUser subject) => new Relationship(this, "manager", subject);
    public Relationship RelateMember(ZedUser subject) => new Relationship(this, "direct_member", subject);

    /// <summary>
    /// Allows all members of subject group to be owners, managers, members of this group (think sub-groups)
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateOwner(ZedGroup subject) => new Relationship(this, "owner", subject.WithSubjectRelation("member"));
    public Relationship RelateManager(ZedGroup subject) => new Relationship(this, "manager", subject.WithSubjectRelation("member"));
    public Relationship RelateMember(ZedGroup subject) => new Relationship(this, "direct_member", subject.WithSubjectRelation("member"));

    /// <summary>
    /// Ban relationship with user and this group
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateBanned(ZedUser subject) => new Relationship(this, "banned", subject);

    /// <summary>
    /// Write these relationships to enable the organization members to search, join without asking, and ask to join
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateSearchers(ZedOrganization subject) => new Relationship(this, "searchers", subject.WithSubjectRelation("member"));
    public Relationship RelateSearchersAllUsers() => new Relationship(this, "searchers", new ResourceReference($"{ZedConfig.Prefix}/user", "*"));
    public Relationship RelateSearchersAnonymousUsers() => new Relationship(this, "searchers", new ResourceReference($"{ZedConfig.Prefix}/anonymous_user", "*"));

    public Relationship RelateJoiners(ZedOrganization subject) => new Relationship(this, "joiners", subject.WithSubjectRelation("member"));
    public Relationship RelateJoiners(ZedPlatform subject) => new Relationship(this, "joiners", subject.WithSubjectRelation("member"));

    public Relationship RelateAskers(ZedOrganization subject) => new Relationship(this, "askers", subject.WithSubjectRelation("member"));
    public Relationship RelateAskers(ZedPlatform subject) => new Relationship(this, "askers", subject.WithSubjectRelation("member"));

    public Relationship RelateInvited(ZedUser subject) => new Relationship(this, "invited", subject);
    public Relationship RelatePending(ZedUser subject) => new Relationship(this, "pending", subject);

    /// <summary>
    /// Enable managers from group to be viewers
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateViewersManagers(ZedGroup subject) => new Relationship(this, "viewers", subject.WithSubjectRelation("manager"));
    /// <summary>
    /// Enable all group members to be viewers
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateViewers(ZedGroup subject) => new Relationship(this, "viewers", subject.WithSubjectRelation("member"));
    /// <summary>
    /// Enable all organization members to be viewers
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateViewers(ZedOrganization subject) => new Relationship(this, "viewers", subject.WithSubjectRelation("member"));

    public Relationship RelateViewersAllUsers() => new Relationship(this, "viewers", new ResourceReference($"{ZedConfig.Prefix}/user", "*"));
    public Relationship RelateViewersAnonymousUsers() => new Relationship(this, "viewers", new ResourceReference($"{ZedConfig.Prefix}/anonymous_user", "*"));

    /// <summary>
    /// Allow all users to post without moderation
    /// </summary>
    /// <returns></returns>
    public Relationship RelatePostWithoutModerationAllUsers() => new Relationship(this, "post_without_moderation", new ResourceReference($"{ZedConfig.Prefix}/user", "*"));

    /// <summary>
    /// Enable an entire group to post without needing moderation
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelatePostWithoutModeration(ZedGroup subject) => new Relationship(this, "post_without_moderation", subject.WithSubjectRelation("member"));

    /// <summary>
    /// Set group managers as content moderators
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateContentModeratorManagers(ZedGroup subject) => new Relationship(this, "content_moderators", subject.WithSubjectRelation("manager"));
    /// <summary>
    /// Allow anybody from group to be a moderator
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateContentModerator(ZedGroup subject) => new Relationship(this, "content_moderators", subject.WithSubjectRelation("member"));

    /// <summary>
    /// Enables members of a custom role to be moderators
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateContentModerator(ZedCustomRole subject) => new Relationship(this, "content_moderators", subject.WithSubjectRelation("member"));

    /// <summary>
    /// Set group managers as content moderators
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateMetadataModeratorManagers(ZedGroup subject) => new Relationship(this, "metadata_moderators", subject.WithSubjectRelation("manager"));
    /// <summary>
    /// Allow anybody from group to be a moderator
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateMetadataModerator(ZedGroup subject) => new Relationship(this, "metadata_moderators", subject.WithSubjectRelation("member"));

    /// <summary>
    /// Enables members of a custom role to be moderators
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelateMetadataModerator(ZedCustomRole subject) => new Relationship(this, "metadata_moderators", subject.WithSubjectRelation("member"));

    /// <summary>
    /// Write this relationship to prevent all users from posting
    /// </summary>
    /// <returns></returns>
    public Relationship RelatePreventPostAllUsers() => new Relationship(this, "prevent_post", new ResourceReference($"{ZedConfig.Prefix}/user", "*"));

    /// <summary>
    /// Allow group managers to post
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelatePostersManagers(ZedGroup subject) => new Relationship(this, "posters", subject.WithSubjectRelation("manager"));
    /// <summary>
    /// Allow group members to post
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelatePosters(ZedGroup subject) => new Relationship(this, "posters", subject.WithSubjectRelation("member"));

    /// <summary>
    /// Allow organization members to post
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    public Relationship RelatePosters(ZedOrganization subject) => new Relationship(this, "posters", subject.WithSubjectRelation("member"));

    /// <summary>
    /// Allow all users to post
    /// </summary>
    /// <returns></returns>
    public Relationship RelatePostersAllUsers() => new Relationship(this, "posters", new ResourceReference($"{ZedConfig.Prefix}/user", "*"));

    public Relationship RelatePageEditorsManagers(ZedGroup subject) => new Relationship(this, "page_editors", subject.WithSubjectRelation("manager"));
    public Relationship RelatePageEditors(ZedGroup subject) => new Relationship(this, "page_editors", subject.WithSubjectRelation("member"));
    public Relationship RelatePageEditors(ZedOrganization subject) => new Relationship(this, "page_editors", subject.WithSubjectRelation("member"));
    public Relationship RelatePageEditors(ZedCustomRole subject) => new Relationship(this, "page_editors", subject.WithSubjectRelation("member"));
    public Relationship RelateMembersViewersManagers(ZedGroup subject) => new Relationship(this, "members_viewers", subject.WithSubjectRelation("manager"));
    public Relationship RelateMembersViewers(ZedGroup subject) => new Relationship(this, "members_viewers", subject.WithSubjectRelation("member"));
    public Relationship RelateMembersViewers(ZedOrganization subject) => new Relationship(this, "members_viewers", subject.WithSubjectRelation("member"));
    public Relationship RelateMembersViewers(ZedCustomRole subject) => new Relationship(this, "members_viewers", subject.WithSubjectRelation("member"));
    public Relationship RelateCommentersManagers(ZedGroup subject) => new Relationship(this, "commenters", subject.WithSubjectRelation("manager"));
    public Relationship RelateCommenters(ZedGroup subject) => new Relationship(this, "commenters", subject.WithSubjectRelation("member"));
    public Relationship RelateCommenters(ZedOrganization subject) => new Relationship(this, "commenters", subject.WithSubjectRelation("member"));
    public Relationship RelateCommenters(ZedCustomRole subject) => new Relationship(this, "commenters", subject.WithSubjectRelation("member"));
    public Relationship RelateCustomRoleManager(ZedGroup subject) => new Relationship(this, "custom_role_manager", subject.WithSubjectRelation("manager"));
    public Relationship RelateCustomRoleManager(ZedCustomRole subject) => new Relationship(this, "custom_role_manager", subject.WithSubjectRelation("member"));
}

public class ZedUser : ResourceReference
{
    private ZedUser(string id) : base($"{ZedConfig.Prefix}/user:{id}")
    {
    }

    // Create Document object
    public static ZedUser WithId(string id) => new ZedUser(id);

    // Platform related permissions
    public Permission PlatformIsAdministrator(ZedPlatform resource) => new Permission(resource, "super_admin", this);

    // Organization related permissions
    public Permission OrgIsAdministrator(ZedOrganization resource) => new Permission(resource, "admin", this);
    public Permission OrgCanCreateGroup(ZedOrganization resource) => new Permission(resource, "create_group", this);

    // Group related permissions 
    public Permission GroupCanBeBannedFrom(ZedGroup resource) => new Permission(resource, "can_be_banned", this);
    public Permission GroupIsMemberOf(ZedGroup resource) => new Permission(resource, "member", this);
    public Permission GroupCanViewInSearch(ZedGroup resource) => new Permission(resource, "view_in_search", this);
    public Permission GroupCanAskToJoin(ZedGroup resource) => new Permission(resource, "ask_to_join", this);
    public Permission GroupCanJoin(ZedGroup resource) => new Permission(resource, "join", this);
    public Permission GroupCanViewPosts(ZedGroup resource) => new Permission(resource, "view_posts", this);
    public Permission GroupCanModerateContent(ZedGroup resource) => new Permission(resource, "moderate_content", this);
    public Permission GroupCanModerateMetadata(ZedGroup resource) => new Permission(resource, "moderate_metadata", this);
    public Permission GroupCanPost(ZedGroup resource) => new Permission(resource, "post", this);
    public Permission GroupCanPostButRequiresModeration(ZedGroup resource) => new Permission(resource, "post_for_moderation", this);
    public Permission GroupCanEditPages(ZedGroup resource) => new Permission(resource, "edit_pages", this);
    public Permission GroupCanViewMembers(ZedGroup resource) => new Permission(resource, "view_members", this);
    public Permission GroupCanAddMember(ZedGroup resource) => new Permission(resource, "add_member", this);
    public Permission GroupCanRemoveMember(ZedGroup resource) => new Permission(resource, "remove_member", this);
    public Permission GroupCanAddAnnouncement(ZedGroup resource) => new Permission(resource, "add_announcement", this);
    public Permission GroupCanRemoveAnnouncement(ZedGroup resource) => new Permission(resource, "remove_announcement", this);
    public Permission GroupCanAddComments(ZedGroup resource) => new Permission(resource, "add_comments", this);
    public Permission GroupCanRemoveComments(ZedGroup resource) => new Permission(resource, "remove_comments", this);
    public Permission GroupCanAddManager(ZedGroup resource) => new Permission(resource, "add_manager", this);
    public Permission GroupCanRemoveManager(ZedGroup resource) => new Permission(resource, "remove_manager", this);
    public Permission GroupCanAddOwner(ZedGroup resource) => new Permission(resource, "add_owner", this);
    public Permission GroupCanRemoveOwner(ZedGroup resource) => new Permission(resource, "remove_owner", this);
    public Permission GroupCanBanUser(ZedGroup resource) => new Permission(resource, "ban_user", this);
    public Permission GroupCanUnbanUser(ZedGroup resource) => new Permission(resource, "unban_user", this);
    public Permission GroupCanManageCustomRole(ZedGroup resource) => new Permission(resource, "manage_custom_role", this);
}



