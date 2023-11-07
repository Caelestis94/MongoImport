using Spectre.Console;
using Spectre.Console.Rendering;

namespace MongoImport.Column;

public class CustomTaskDescColumn : ProgressColumn
{
	public override IRenderable Render(RenderOptions options, ProgressTask task, TimeSpan deltaTime)
	{
		if (task.IsFinished)
		{
			return new Markup($"{task.Description}[green] - Terminée![/]").Justify(Justify.Left);
		}
		
		return new Markup($"[yellow]{task.Description}[/]").Justify(Justify.Left);
	}
}