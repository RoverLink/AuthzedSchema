using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using SpiceDb.Models;

namespace ArchimedesPermissionsTest;

public static class ZedConfig
{
	public static string Prefix = "archtest";
}

public class ZedPlatform : ResourceReference
{
	private ZedPlatform(string id) : base($"{ZedConfig.Prefix}/platform:{id}")
	{
	}

	public static ZedPlatform WithId(string id) => new ZedPlatform(id);
}

public class ZedOrganization : ResourceReference
{
	private ZedOrganization(string id) : base($"{ZedConfig.Prefix}/organization:{id}")
	{
	}

	public static ZedOrganization WithId(string id) => new ZedOrganization(id);
}

public class ZedGroup : ResourceReference
{
	private ZedGroup(string id) : base($"{ZedConfig.Prefix}/group:{id}")
	{
	}

	// Create Group object
	public static ZedGroup WithId(string id) => new ZedGroup(id);
}

public class ZedUser : ResourceReference
{
	private ZedUser(string id) : base($"{ZedConfig.Prefix}/user:{id}")
	{
	}

	// Create Document object
	public static ZedUser WithId(string id) => new ZedUser(id);

	// Platform related permissions
	public Permission IsAdministrator(ZedPlatform resource) => new Permission(resource, "super_admin", this);

    // Organization related permissions
    public Permission IsAdministrator(ZedOrganization resource) => new Permission(resource, "admin", this);
    public Permission CanCreateGroup(ZedOrganization resource) => new Permission(resource, "create_group", this);

    // Group related permissions 
    public Permission CanBeBannedFrom(ZedGroup resource) => new Permission(resource, "can_be_banned", this);
	public Permission IsMemberOf(ZedGroup resource) => new Permission(resource, "member", this);
	public Permission CanViewInSearch(ZedGroup resource) => new Permission(resource, "view_in_search", this);
	public Permission CanAskToJoin(ZedGroup resource) => new Permission(resource, "ask_to_join", this);
	public Permission CanJoin(ZedGroup resource) => new Permission(resource, "join", this);
	public Permission CanViewPosts(ZedGroup resource) => new Permission(resource, "view_posts", this);

}



