﻿namespace ManyToManyRemoveAndAddTest.Models;

public partial class Post
{
	public Post(string title, string text)
	{
		Title = title;
		Text = text;
		Tags = new HashSet<Tag>();
	}

	public int Id { get; set; }
	public string Title { get; set; }
	public string Text { get; set; }

	public virtual ICollection<Tag> Tags { get; set; }
}
