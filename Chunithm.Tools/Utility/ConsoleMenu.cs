using System;
using System.Collections.Generic;
using System.Text;

namespace Nim.Console
{
    public class ConsoleMenu
    {
        int lasLen = 0;
        bool canWork = true;
        bool confimMode = false;
        bool inputMode = false;
        Item selected;

        public Item Main { get; }
        public string ConfirmText { get; set; } = "Are you sure? [Y/N]";

        public Item Selected
        {
            get => selected;
            set
            {
                selected = value;

                if (selected.ActionOnSelected)
                {
                    selected.Action?.Invoke();
                }
            }
        }

        public Item Current { get; private set; }
        public List<Item> Items { get; }
        public List<string> Log { get; } = new List<string>();

        public ConsoleMenu() : this("", new Item[0])
        {
        }

        public ConsoleMenu(Item[] items) : this("", items)
        {
        }

        public ConsoleMenu(string title, Item[] items)
        {
            Main = new Item(title, items);
            Items = new List<Item>(items);

            Current = Main;

            if (items.Length > 0)
            {
                Selected = items[0];
            }
            else
            {
                Selected = Main;
            }
        }

        // Worker cycle
        public void Begin()
        {
            System.Console.CursorVisible = false;

            while (canWork)
            {
                Refresh();

            escape: var info = System.Console.ReadKey(true);

                switch (info.Key)
                {
                    case System.ConsoleKey.Backspace:
                        {
                            if (Current.Parent != null)
                            {
                                foreach (var itm in Current.Parent.Items)
                                {
                                    if (itm == Current)
                                    {
                                        Selected = itm;

                                        break;
                                    }
                                }

                                Current = Current.Parent;
                            }

                            break;
                        }
                    case System.ConsoleKey.Escape:
                        {
                            Escape();
                            break;
                        }
                    case System.ConsoleKey.Enter:
                        {
                            if (Selected is InputItem)
                            {
                                inputMode = true;

                                break;
                            }


                            if (Selected.ActionIfConfirmed)
                            {
                                confimMode = true;

                                break;
                            }

                            Selected.Action?.Invoke();

                            if (Selected.Items.Count > 0 && !selected.IsToggle)
                            {
                                Current = Selected;
                                Selected = Current.Items[0];
                            }

                            break;
                        }
                    case System.ConsoleKey.UpArrow:
                        {
                            var sel = GetIndex();

                            if (sel > -1)
                            {
                                sel -= lasLen;

                                if (sel < 0)
                                {
                                    sel += Current.Items.Count;
                                }
                            }

                            Selected = Current.Items[sel];

                            break;
                        }
                    case System.ConsoleKey.DownArrow:
                        {
                            var sel = GetIndex();

                            if (sel > -1)
                            {
                                sel += lasLen;

                                if (sel >= Current.Items.Count)
                                {
                                    sel -= Current.Items.Count;
                                }
                            }

                            Selected = Current.Items[sel];

                            break;
                        }
                    case System.ConsoleKey.LeftArrow:
                        {
                            var sel = GetIndex();

                            if (sel > -1)
                            {
                                sel--;

                                if (sel < 0)
                                {
                                    sel = Current.Items.Count - 1;
                                }
                            }

                            Selected = Current.Items[sel];

                            break;
                        }
                    case System.ConsoleKey.RightArrow:
                        {
                            var sel = GetIndex();

                            if (sel > -1)
                            {
                                sel++;

                                if (sel == Current.Items.Count)
                                {
                                    sel = 0;
                                }
                            }

                            Selected = Current.Items[sel];

                            break;
                        }
                    case System.ConsoleKey.Delete:
                        {
                            Log.Clear();
                            break;
                        }
                    default:
                        {
                            if (confimMode)
                            {
                                if (info.Key == System.ConsoleKey.Y)
                                {
                                    Selected.Action?.Invoke();
                                }

                                confimMode = false;

                                break;
                            }

                            goto escape;
                        }
                }
            }

            int GetIndex()
            {
                var sel = -1;

                for (var i = 0; i < Current.Items.Count; ++i)
                {
                    if (Selected == Current.Items[i])
                    {
                        sel = i;

                        break;
                    }
                }

                if (sel == -1)
                {
                    if (Current.Items.Count > 0)
                    {
                        sel = 0;
                    }
                }

                return sel;
            }
        }

        public void Escape()
        {
            Log.Clear();
            Current = Main;

            if (Main.Items.Count > 0)
            {
                Selected = Main.Items[0];
            }
        }

        // Drawing
        public void Refresh()
        {
            if (inputMode)
            {
                var inp = Selected as InputItem;

                System.Console.BackgroundColor = System.ConsoleColor.Black;
                System.Console.ForegroundColor = System.ConsoleColor.Green;
                System.Console.Clear();
                System.Console.Write(inp.Title + ": ");

                System.Console.ResetColor();

                inp.Value = System.Console.ReadLine();

                inputMode = false;

                inp.Action?.Invoke(inp.Value);

                return;
            }


            if (confimMode)
            {
                System.Console.BackgroundColor = System.ConsoleColor.Gray;
                System.Console.ForegroundColor = System.ConsoleColor.DarkRed;
                System.Console.Clear();
                System.Console.WriteLine();
                System.Console.WriteLine();
                System.Console.WriteLine(Selected.Name.PadLeft(40));
                System.Console.WriteLine();
                System.Console.WriteLine(ConfirmText.PadLeft(40));

                System.Console.ResetColor();

                return;
            }

            System.Console.Clear();

            // Drawing nav
            var nav = Current.Name;
            var cur = Current.Parent;
            var cursor = 0;

            while (cur != null)
            {
                nav = cur.Name + " => " + nav;

                cur = cur.Parent;
            }

            System.Console.WriteLine(nav);
            System.Console.WriteLine();

            var max_width = -1;

            for (var i = 0; i < Current.Items.Count; i++)
            {
                var itm = Current.Items[i];

                if (itm.Name.Length > max_width)
                {
                    max_width = itm.Name.Length;
                }
            }

            var col_space = 5;
            var len = System.Console.WindowWidth / (max_width + col_space) - 1;

            if (Current.MaxColumns > 0 && Current.MaxColumns < len)
            {
                len = Current.MaxColumns;
            }

            lasLen = len;

            for (var i = 0; i < Current.Items.Count; i += len)
            {
                for (var j = 0; j < len && i + j < Current.Items.Count; ++j)
                {
                    var itm = Current.Items[i + j];

                    if (itm == Selected)
                    {
                        System.Console.BackgroundColor = System.ConsoleColor.Gray;
                        System.Console.ForegroundColor = System.ConsoleColor.Black;
                    }

                    var name = itm.Name.PadRight(max_width + 2);

                    if (name.Length >= System.Console.LargestWindowWidth)
                    {
                        name = name.Substring(0, System.Console.LargestWindowWidth - 5) + "...";
                    }

                    System.Console.Write(name);

                    System.Console.ResetColor();

                    System.Console.Write("".PadRight(col_space));
                }

                System.Console.WriteLine();

                var tmp = System.Console.CursorTop;

                if (tmp > cursor)
                {
                    cursor = tmp;
                }
            }

            System.Console.CursorTop = cursor + 1;
            System.Console.CursorLeft = 0;

            if (Log.Count > 0)
            {
                var sb = new StringBuilder();
                sb.AppendLine("______________________________");
                sb.AppendLine("");

                foreach (var itm in Log)
                {
                    sb.AppendLine(itm);
                }

                System.Console.Write(sb);
            }
        }

        //
        public void Close()
        {
            canWork = false;
        }

        public void WriteLine(string str)
        {
            Log.Add(str);
            Refresh();
        }

        public class InputItem : Item
        {
            public new Action<string> Action { get; set; }
            public string Value { get; set; } = "";
            public string Title { get; set; }

            public InputItem(string name, string title, Action<string> action) : base(name, null as Action, 0)
            {
                Title = title;
                Action = action;
            }
        }

        public class Item
        {
            public Item Parent { get; private set; } = null;
            public string Name { get; set; }
            public Action Action { get; set; }
            public IReadOnlyList<Item> Items { get; }
            public object Tag { get; set; }
            public bool ActionOnSelected { get; set; } = false;
            public bool ActionIfConfirmed { get; set; } = false;
            public int MaxColumns { get; set; }
            public bool IsToggle { get; set; }

            public Item(string name, Action action, int maxColumns = 0) : this(name, action, new Item[0], maxColumns)
            {
            }

            public Item(string name, Item[] items, int maxColumns = 0) : this(name, null, items, maxColumns)
            {
            }

            public Item(string name) : this(name, Array.Empty<Item>(), 0)
            {
            }

            public Item(string name, Action action, Item[] items, int maxColumns = 0)
            {
                Name = name;
                MaxColumns = maxColumns;
                Action = action;
                Items = new List<Item>(items);

                foreach (var itm in items)
                {
                    itm.Parent = this;
                }
            }

            public void Add(Item item)
            {
                item.Parent = this;

                ((List<Item>)Items).Add(item);
            }

            public Item Add(string name, Action a, int maxColumns = 0)
            {
                var itm = new Item(name, a, maxColumns) { Parent = this };

                ((List<Item>)Items).Add(itm);

                return itm;
            }

            public Item Add(string name, Action a, object tag, int maxColumns = 0)
            {
                var itm = new Item(name, a, maxColumns) { Parent = this, Tag = tag };

                ((List<Item>)Items).Add(itm);

                return itm;
            }

            public void Clear()
            {
                ((List<Item>)Items).Clear();
            }
        }
    }
}