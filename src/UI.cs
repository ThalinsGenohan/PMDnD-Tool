using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace Thalins.PMDnD
{
    public class UI
    {
        // Fields
        public RenderWindow Window { get; }
        private readonly Font _font;
        private string _currentTab;
        private bool _running = true;
        private readonly List<Button> _buttons;
        private readonly Dictionary<string, Tab> _tabs;

        // Constructors
        public UI()
        {
            Window = new RenderWindow(new VideoMode(800, 600), "Test", Styles.Close | Styles.Titlebar);
            Window.SetFramerateLimit(60);

            Window.MouseButtonPressed += new EventHandler<MouseButtonEventArgs>(OnClick);
            Window.Closed += new EventHandler(OnClose);
            Window.MouseMoved += new EventHandler<MouseMoveEventArgs>(OnMouseMove);

            _font = new Font("WONDERMAIL.TTF");

            _tabs = new Dictionary<string, Tab>
            {
                { "main", new Tab() },
                { "players", new Tab() }
            };

            _currentTab = "main";

            _buttons = new List<Button>
            {
                new Button()
                {
                    Font = _font,
                    Color = Color.Black,
                    OutlineColor = Color.Green,
                    OutlineThickness = 3.0f,
                    Position = new Vector2f(100, 50),
                    Size = new Vector2f(100, 50),
                    Text = "Main",
                    ClickEvent = () => { changeTab("main"); }
                },
                new Button()
                {
                    Font = _font,
                    Color = Color.Black,
                    OutlineColor = Color.Green,
                    OutlineThickness = 3.0f,
                    Position = new Vector2f(203, 50),
                    Size = new Vector2f(100, 50),
                    Text = "Players",
                    ClickEvent = () => { changeTab("players"); }
                }
            };
        }

        // Enums
        public enum Style
        {
            Rectangle,
            Slanted
        }

        // Events
        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Button != Mouse.Button.Left)
            {
                return;
            }

            foreach (var button in _buttons)
            {
                if (button.Bounds.Contains(e.X, e.Y))
                {
                    button.OnClick();
                }
            }
        }
        private void OnClose(object sender, EventArgs e)
        {
            Window.Close();
            _running = false;
        }
        private void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            foreach (var button in _buttons)
            {
                if (button.Bounds.Contains(e.X, e.Y))
                {
                    button.OnHover();
                }
                else button.NoHover();
            }
        }

        // Methods
        public void Run()
        {
            while (_running)
            {
                Window.DispatchEvents();

                Window.Clear();

                foreach (var button in _buttons) Window.Draw(button);
                Window.Draw(_tabs[_currentTab]);

                Window.Display();
            }
        }
        private void changeTab(string tab)
        {
            _currentTab = tab;
        }

        // Classes
        private class Bar : Drawable
        {
            // Fields
            public int Max = 100;
            public int Min = 0;
            public BarShape Shape;
            private readonly ConvexShape _background = new ConvexShape();
            private readonly ConvexShape _fill = new ConvexShape();
            private int _current = 100;
            private Vector2f _position;
            private Vector2f _size;

            // Constructors
            public Bar(Vector2f position, Vector2f size, Color color, BarShape shape = BarShape.Rectangle)
            {
                Shape = shape;
                setPositionAndSize(position, size);

                _fill.FillColor = color;
                _background.FillColor = new Color(0x55, 0x55, 0x55);
                _background.OutlineColor = Color.White;
                _background.OutlineThickness = 2.0f;
            }
            public Bar(float posX, float posY, float width, float height, Color color, BarShape shape = BarShape.Rectangle)
            {
                Shape = shape;
                setPositionAndSize(new Vector2f(posX, posY), new Vector2f(width, height));

                _fill.FillColor = color;
                _background.FillColor = new Color(0x55, 0x55, 0x55);
                _background.OutlineColor = Color.White;
                _background.OutlineThickness = 2.0f;
            }

            // Enums
            public enum BarShape
            {
                Rectangle,
                Slanted,
                Circle
            }

            // Interface Implementations
            public void Draw(RenderTarget target, RenderStates states)
            {
                target.Draw(_background, states);
                target.Draw(_fill, states);
            }

            // Properties
            public int Current { get => _current; set => setFill(value); }
            public Vector2f Position { get => _position; set => setPositionAndSize(value, Size); }
            public Vector2f Size { get => _size; set => setPositionAndSize(Position, value); }

            // Methods
            private void setFill(int fill)
            {
                _current = fill;

                var topPoint = new Vector2f(Position.X + Size.X * ((float)Current / (float)Max), Position.Y);
                var bottomPoint = new Vector2f(Position.X + Size.X * ((float)Current / (float)Max) - Size.Y, Position.Y + Size.Y);

                if (bottomPoint.X <= Position.X)
                {
                    bottomPoint.X = Position.X;
                    bottomPoint.Y = Position.Y + (topPoint.X - Position.X);
                    _fill.SetPoint(3, bottomPoint);
                }
                else _fill.SetPoint(3, new Vector2f(Position.X, Position.Y + Size.Y));

                _fill.SetPoint(1, topPoint);
                _fill.SetPoint(2, bottomPoint);
            }
            private void setPositionAndSize(Vector2f position, Vector2f size)
            {
                _position = position;
                _size = size;

                _background.SetPointCount(4);
                _background.SetPoint(0, position);
                _background.SetPoint(1, new Vector2f(position.X + size.X, position.Y));
                switch (Shape)
                {
                    case BarShape.Slanted:
                        _background.SetPoint(2, new Vector2f(position.X + size.X - size.Y, position.Y + size.Y));
                        break;

                    case BarShape.Rectangle:
                    default:
                        _background.SetPoint(2, new Vector2f(position.X + size.X, position.Y + size.Y));
                        break;
                }
                _background.SetPoint(3, new Vector2f(position.X, position.Y + size.Y));

                _fill.SetPointCount(4);
                _fill.SetPoint(0, position);
                _fill.SetPoint(1, new Vector2f(position.X + size.X, position.Y));
                switch (Shape)
                {
                    case BarShape.Slanted:
                        _fill.SetPoint(2, new Vector2f(position.X + size.X - size.Y, position.Y + size.Y));
                        break;

                    case BarShape.Rectangle:
                    default:
                        _fill.SetPoint(2, new Vector2f(position.X + size.X, position.Y + size.Y));
                        break;
                }
                _fill.SetPoint(3, new Vector2f(position.X, position.Y + size.Y));
            }
        }
        private class Button : Drawable
        {
            // Fields
            public bool Toggle = false;
            private readonly Texture _cursorTexture;
            private readonly Text _text;
            private readonly RectangleShape _shape;
            private bool _active;
            private Sprite _cursor;
            private Font _font;
            private bool _hovering;

            // Constructors
            public Button()
            {
                _text = new Text
                {
                    CharacterSize = 36
                };
                _shape = new RectangleShape()
                {
                    OutlineColor = Color.White
                };
            }
            public Button(Vector2f position, Vector2f size)
            {
                _text = new Text
                {
                    CharacterSize = 36
                };
                _shape = new RectangleShape(size)
                {
                    Position = position,
                    OutlineColor = Color.White
                };
            }
            public Button(float posX, float posY, float sizeX, float sizeY)
            {
                _text = new Text
                {
                    CharacterSize = 36
                };
                _shape = new RectangleShape(new Vector2f(sizeX, sizeY))
                {
                    Position = new Vector2f(posX, posY),
                    OutlineColor = Color.White
                };
            }

            // Delegates
            public delegate void ButtonDelegate();

            public ButtonDelegate ClickEvent;

            // Events
            public void OnClick()
            {
                if (Toggle)
                {
                    _active = !_active;
                }

                ClickEvent();
            }
            public void OnHover()
            {
                if (Hovering) return;

                Hovering = true;
                Color = new Color(0x66, 0x66, 0x66);
            }
            public void NoHover()
            {
                if (!Hovering) return;

                Hovering = false;
                Color = Color.Black;
            }

            // Interface Implementations
            public void Draw(RenderTarget target, RenderStates states)
            {
                target.Draw(_shape, states);
                target.Draw(_text, states);
            }

            // Properties
            public FloatRect Bounds => _shape.GetGlobalBounds();
            public Color Color { get => _shape.FillColor; set => _shape.FillColor = value; }
            public Font Font { get => _font; set { _font = value; _text.Font = _font; } }
            public bool Hovering { get => _hovering; set => _hovering = value; }
            public Color OutlineColor { get => _shape.OutlineColor; set => _shape.OutlineColor = value; }
            public float OutlineThickness { get => _shape.OutlineThickness; set => _shape.OutlineThickness = value; }
            public Vector2f Position { get => _shape.Position; set => _shape.Position = value; }
            public Vector2f Size { get => _shape.Size; set => _shape.Size = value; }
            public string Text { get => _text.DisplayedString; set => setString(value); }

            // Methods
            private void setString(string text)
            {
                _text.DisplayedString = text;

                var bounds = _text.GetLocalBounds();
                _text.Origin = new Vector2f(bounds.Left + bounds.Width / 2.0f, bounds.Top + bounds.Height / 2.0f);
                _text.Position = new Vector2f(_shape.Position.X + _shape.Size.X / 2.0f, _shape.Position.Y + _shape.Size.Y / 2.0f);
            }
        }
        private class Character : Drawable
        {
            private Vector2f Position;
            private readonly RectangleShape PortraitBorder;
            private readonly Sprite Portrait;
            private readonly Bar HPBar;
            private readonly Dictionary<string, Stat> Stats;

            public Character(Vector2f position)
            {
                Position = position;

                PortraitBorder = new RectangleShape(new Vector2f(100, 100))
                {
                    Position = position,
                    FillColor = Color.Black,
                    OutlineThickness = 2.0f,
                    OutlineColor = Color.White
                };

                Portrait = new Sprite();

                HPBar = new Bar(new Vector2f(Position.X + PortraitBorder.Size.X + PortraitBorder.OutlineThickness, Position.Y), new Vector2f(250, 30), Color.Red, Bar.BarShape.Slanted);

                float statPosX = position.X + PortraitBorder.Size.X + 2;
                float statSize = (HPBar.Size.X / 4) - (HPBar.Size.Y / 4) - 0.5f;

                Stats = new Dictionary<string, Stat>
                {
                    { "Strength", new Stat(new Vector2f(statPosX, position.Y + HPBar.Size.Y + 2), new Vector2f(statSize, HPBar.Size.Y), new Color(0xff, 0xa5, 0), true) },
                    { "Special", new Stat(new Vector2f(statPosX + statSize, position.Y + HPBar.Size.Y + 2), new Vector2f(statSize, HPBar.Size.Y), Color.Blue) },
                    { "Speed", new Stat(new Vector2f(statPosX + statSize * 2, position.Y + HPBar.Size.Y + 2), new Vector2f(statSize, HPBar.Size.Y), Color.Green) },
                    { "Vitality", new Stat(new Vector2f(statPosX + statSize * 3, position.Y + HPBar.Size.Y + 2), new Vector2f(statSize, HPBar.Size.Y), Color.Red) }
                };
            }

            public void Draw(RenderTarget target, RenderStates states)
            {
                target.Draw(this.PortraitBorder, states);
                ////target.Draw(Portrait, states);
                target.Draw(HPBar, states);
                foreach (var stat in this.Stats.Values)
                {
                    target.Draw(stat, states);
                }
            }
        }
        private class Stat : Drawable
        {
            // FIELDS

            // Public Fields
            public ConvexShape Shape;

            public Vector2f Position { get => _position; set => setPositionAndSize(value, _size); }
            public Vector2f Size { get => _size; set => setPositionAndSize(_position, value); }
            public Color Color { get => Shape.FillColor; set => Shape.FillColor = value; }

            // Private Fields
            private readonly bool _flat;

            private Vector2f _size;
            private Vector2f _position;

            // CONSTRUCTORS
            public Stat(Vector2f position, Vector2f size, Color color, bool flat = false)
            {
                Shape = new ConvexShape(4);
                _flat = flat;
                Position = position;
                Size = size;
                Shape.FillColor = color;

                Shape.OutlineThickness = 2.0f;
            }

            // INTERFACES

            // Public Interfaces

            // METHODS

            // Public Methods
            public void Draw(RenderTarget target, RenderStates states)
            {
                target.Draw(Shape, states);
            }

            // Private Methods
            private void setPositionAndSize(Vector2f position, Vector2f size)
            {
                _position = position;
                _size = size;

                Shape.SetPoint(0, new Vector2f(position.X, position.Y));
                Shape.SetPoint(1, new Vector2f(position.X + size.X, position.Y));
                Shape.SetPoint(2, new Vector2f(position.X + size.X - size.Y, position.Y + size.Y));
                if (_flat) Shape.SetPoint(3, new Vector2f(position.X, position.Y + size.Y));
                else Shape.SetPoint(3, new Vector2f(position.X - size.Y, position.Y + size.Y));
            }
        }
        private class Tab : Drawable
        {
            public List<Drawable> Drawables;

            public Tab()
            {
                Drawables = new List<Drawable>();
            }

            public void Draw(RenderTarget target, RenderStates states)
            {
                foreach (var drawable in Drawables)
                {
                    target.Draw(drawable, states);
                }
            }
        }
    }
}