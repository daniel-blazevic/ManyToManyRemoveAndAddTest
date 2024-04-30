using ManyToManyRemoveAndAddTest.Data;
using ManyToManyRemoveAndAddTest.Models;
using Microsoft.EntityFrameworkCore;

namespace ManyToManyRemoveAndAddTest;

internal class Program
{
	//Add one Post and two Tags to the database
	static async Task SeedDataAsync()
	{
		using var ctx = new Context();

		ctx.Database.EnsureDeleted();
		ctx.Database.EnsureCreated();

		var post = new Post() { Title = "Post 1 Title", Text = "Post 1 Text" };

		var tag1 = new Tag() { Text = "Tag 1 Text" };
		var tag2 = new Tag() { Text = "Tag 2 Text" };

		post.Tags.Add(tag1);
		post.Tags.Add(tag2);

		ctx.Add(post);

		await ctx.SaveChangesAsync();
	}

	//Get the count of Tags associated with the first Post
	static async Task<int> GetTagCount()
	{
		using var ctx = new Context();

		return await ctx.Posts.Select(x => x.Tags.Count).FirstAsync();		
	}

	//Remove and add the same Tag from the first Post
	static async Task RemoveAndAddSameTagFromPostAsync(bool callDetectChanges)
	{
		using var ctx = new Context();

		var post = ctx.Posts.Include(x => x.Tags).First();

		var tag = post.Tags.First();

		post.Tags.Remove(tag);

		if (callDetectChanges)
		{
			ctx.ChangeTracker.DetectChanges();
		}

		post.Tags.Add(tag);

		await ctx.SaveChangesAsync();
	}

	//Add and remove the same Tag from the first Post
	static async Task AddAndRemoveSameTagFromPostAsync(bool callDetectChanges)
	{
		using var ctx = new Context();

		var post = ctx.Posts.Include(x => x.Tags).First();

		var tag = new Tag() { Text = "Tag 3 Text" };

		post.Tags.Add(tag);

		if (callDetectChanges)
		{
			ctx.ChangeTracker.DetectChanges();
		}

		post.Tags.Remove(tag);

		await ctx.SaveChangesAsync();
	}

	static async Task TestRemoveAndAddAsync(bool callDetectChanges)
	{
		Console.WriteLine($"{Environment.NewLine}Test of removing and then adding of same tag {(callDetectChanges ? "with" : "without")} DetectChanges()");

		Console.WriteLine($"Tag count before: {await GetTagCount()}");

		await RemoveAndAddSameTagFromPostAsync(callDetectChanges);

		Console.WriteLine($"Tag count after:  {await GetTagCount()}");
	}

	static async Task TestAddAndRemoveAsync(bool callDetectChanges)
	{
		Console.WriteLine($"{Environment.NewLine}Test of adding and then removing of same tag {(callDetectChanges ? "with" : "without")} DetectChanges()");

		Console.WriteLine($"Tag count before: {await GetTagCount()}");

		await AddAndRemoveSameTagFromPostAsync(callDetectChanges);

		Console.WriteLine($"Tag count after:  {await GetTagCount()}");
	}

	static async Task Main(string[] args)
	{
		await SeedDataAsync();

		await TestAddAndRemoveAsync(false);
		await TestAddAndRemoveAsync(true);
		
		await TestRemoveAndAddAsync(false);
		await TestRemoveAndAddAsync(true);
	}
}
