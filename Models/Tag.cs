namespace ManyToManyRemoveAndAddTest.Models;

public partial class Tag
{
	public Tag(string text)
	{
		Text = text;
		Posts = new HashSet<Post>();
	}

	public int Id { get; set; }
	public string Text { get; set; }

	public virtual ICollection<Post> Posts { get; set; }
}
