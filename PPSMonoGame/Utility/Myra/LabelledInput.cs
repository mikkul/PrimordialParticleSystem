using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace PPSMonoGame.Utility.Myra
{
	class LabelledInput : Grid
	{
		public LabelledInput()
		{
			this.RowsProportions.Add(new Proportion());
			this.ColumnsProportions.Add(new Proportion());
			this.ColumnsProportions.Add(new Proportion(ProportionType.Fill));

			Label = new Label
			{
				GridColumn = 0,
				GridRow = 0,
			};
			Input = new TextBox
			{
				GridColumn = 1,
				GridRow = 0,
				Margin = new Thickness(3, 0),
			};

			this.Widgets.Add(Label);
			this.Widgets.Add(Input);
		}

		public string Text
		{
			get => Label.Text;
			set
			{
				Label.Text = value;
			}
		}

		public string Value
		{
			get => Input.Text;
			set
			{
				Input.Text = value;
			}
		}

		public Label Label { get; protected set; }
		public TextBox Input { get; protected set; }
	}
}
