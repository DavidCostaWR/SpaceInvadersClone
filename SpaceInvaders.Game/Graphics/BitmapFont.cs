
using SpaceInvaders.Game.Domain;
using Rectangle = SpaceInvaders.Game.Domain.Rectangle;

namespace SpaceInvaders.Game.Graphics
{
    /// <summary>
    /// Simple bitmap font for retro text rendering
    /// </summary>
    public class BitmapFont
    {
        private readonly Dictionary<char, BitmapSprite> _characters;
        private readonly int _characterWidth;
        private readonly int _characterHeight;
        private readonly int _spacing;

        public int CharacterWidth => _characterWidth;
        public int CharacterHeight => _characterHeight;

        public BitmapFont()
        {
            _characterWidth = 5;
            _characterHeight = 7;
            _spacing = 1;
            _characters = new Dictionary<char, BitmapSprite>();
            LoadCharacters();
        }

        private void LoadCharacters()
        {
            // Define bitmap patterns for each characters
            // These are 5 x 7 pixel characters
            _characters['A'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "X   X",
                "XXXXX",
                "X   X",
                "X   X",
                "X   X"
            });

            _characters['B'] = new BitmapSprite(new[] {
                "XXXX ",
                "X   X",
                "X   X",
                "XXXX ",
                "X   X",
                "X   X",
                "XXXX "
            });

            _characters['C'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "X    ",
                "X    ",
                "X    ",
                "X   X",
                " XXX "
            });

            _characters['D'] = new BitmapSprite(new[] {
                "XXXX ",
                "X   X",
                "X   X",
                "X   X",
                "X   X",
                "X   X",
                "XXXX "
            });

            _characters['E'] = new BitmapSprite(new[] {
                "XXXXX",
                "X    ",
                "X    ",
                "XXXX ",
                "X    ",
                "X    ",
                "XXXXX"
            });

            _characters['F'] = new BitmapSprite(new[] {
                "XXXXX",
                "X    ",
                "X    ",
                "XXXX ",
                "X    ",
                "X    ",
                "X    "
            });

            _characters['G'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "X    ",
                "X  XX",
                "X   X",
                "X   X",
                " XXX "
            });

            _characters['H'] = new BitmapSprite(new[] {
                "X   X",
                "X   X",
                "X   X",
                "XXXXX",
                "X   X",
                "X   X",
                "X   X"
            });

            _characters['I'] = new BitmapSprite(new[] {
                "XXXXX",
                "  X  ",
                "  X  ",
                "  X  ",
                "  X  ",
                "  X  ",
                "XXXXX"
            });

            _characters['J'] = new BitmapSprite(new[] {
                "  XXX",
                "    X",
                "    X",
                "    X",
                "X   X",
                "X   X",
                " XXX "
            });

            _characters['K'] = new BitmapSprite(new[] {
                "X   X",
                "X  X ",
                "X X  ",
                "XX   ",
                "X X  ",
                "X  X ",
                "X   X"
            });

            _characters['L'] = new BitmapSprite(new[] {
                "X    ",
                "X    ",
                "X    ",
                "X    ",
                "X    ",
                "X    ",
                "XXXXX"
            });

            _characters['M'] = new BitmapSprite(new[] {
                "X   X",
                "XX XX",
                "X X X",
                "X   X",
                "X   X",
                "X   X",
                "X   X"
            });

            _characters['N'] = new BitmapSprite(new[] {
                "X   X",
                "XX  X",
                "X X X",
                "X  XX",
                "X   X",
                "X   X",
                "X   X"
            });

            _characters['O'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "X   X",
                "X   X",
                "X   X",
                "X   X",
                " XXX "
            });

            _characters['P'] = new BitmapSprite(new[] {
                "XXXX ",
                "X   X",
                "X   X",
                "XXXX ",
                "X    ",
                "X    ",
                "X    "
            });

            _characters['Q'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "X   X",
                "X   X",
                "X X X",
                "X  XX",
                " XXXX"
            });

            _characters['R'] = new BitmapSprite(new[] {
                "XXXX ",
                "X   X",
                "X   X",
                "XXXX ",
                "X  X ",
                "X   X",
                "X   X"
            });

            _characters['S'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "X    ",
                " XXX ",
                "    X",
                "X   X",
                " XXX "
            });

            _characters['T'] = new BitmapSprite(new[] {
                "XXXXX",
                "  X  ",
                "  X  ",
                "  X  ",
                "  X  ",
                "  X  ",
                "  X  "
            });

            _characters['U'] = new BitmapSprite(new[] {
                "X   X",
                "X   X",
                "X   X",
                "X   X",
                "X   X",
                "X   X",
                " XXX "
            });

            _characters['V'] = new BitmapSprite(new[] {
                "X   X",
                "X   X",
                "X   X",
                "X   X",
                " X X ",
                " X X ",
                "  X  "
            });

            _characters['W'] = new BitmapSprite(new[] {
                "X   X",
                "X   X",
                "X   X",
                "X X X",
                "X X X",
                "XX XX",
                "X   X"
            });

            _characters['X'] = new BitmapSprite(new[] {
                "X   X",
                "X   X",
                " X X ",
                "  X  ",
                " X X ",
                "X   X",
                "X   X"
            });

            _characters['Y'] = new BitmapSprite(new[] {
                "X   X",
                "X   X",
                " X X ",
                "  X  ",
                "  X  ",
                "  X  ",
                "  X  "
            });

            _characters['Z'] = new BitmapSprite(new[] {
                "XXXXX",
                "    X",
                "   X ",
                "  X  ",
                " X   ",
                "X    ",
                "XXXXX"
            });

            // Numbers
            _characters['0'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "X  XX",
                "X X X",
                "XX  X",
                "X   X",
                " XXX "
            });

            _characters['1'] = new BitmapSprite(new[] {
                "  X  ",
                " XX  ",
                "  X  ",
                "  X  ",
                "  X  ",
                "  X  ",
                "XXXXX"
            });

            _characters['2'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "    X",
                "   X ",
                "  X  ",
                " X   ",
                "XXXXX"
            });

            _characters['3'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "    X",
                "  XX ",
                "    X",
                "X   X",
                " XXX "
            });

            _characters['4'] = new BitmapSprite(new[] {
                "   X ",
                "  XX ",
                " X X ",
                "X  X ",
                "XXXXX",
                "   X ",
                "   X "
            });

            _characters['5'] = new BitmapSprite(new[] {
                "XXXXX",
                "X    ",
                "X    ",
                "XXXX ",
                "    X",
                "X   X",
                " XXX "
            });

            _characters['6'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "X    ",
                "XXXX ",
                "X   X",
                "X   X",
                " XXX "
            });

            _characters['7'] = new BitmapSprite(new[] {
                "XXXXX",
                "    X",
                "   X ",
                "  X  ",
                " X   ",
                " X   ",
                " X   "
            });

            _characters['8'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "X   X",
                " XXX ",
                "X   X",
                "X   X",
                " XXX "
            });

            _characters['9'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "X   X",
                " XXXX",
                "    X",
                "X   X",
                " XXX "
            });

            // Special characters
            _characters[' '] = new BitmapSprite(new[] {
                "     ",
                "     ",
                "     ",
                "     ",
                "     ",
                "     ",
                "     "
            });

            _characters['!'] = new BitmapSprite(new[] {
                "  X  ",
                "  X  ",
                "  X  ",
                "  X  ",
                "  X  ",
                "     ",
                "  X  "
            });

            _characters['?'] = new BitmapSprite(new[] {
                " XXX ",
                "X   X",
                "    X",
                "   X ",
                "  X  ",
                "     ",
                "  X  "
            });

            _characters['>'] = new BitmapSprite(new[] {
                "X    ",
                " X   ",
                "  X  ",
                "   X ",
                "  X  ",
                " X   ",
                "X    "
            });

            _characters['<'] = new BitmapSprite(new[] {
                "    X",
                "   X ",
                "  X  ",
                " X   ",
                "  X  ",
                "   X ",
                "    X"
            });

            _characters[':'] = new BitmapSprite(new[] {
                "     ",
                "  X  ",
                "  X  ",
                "     ",
                "  X  ",
                "  X  ",
                "     "
            });

            _characters['-'] = new BitmapSprite(new[] {
                "     ",
                "     ",
                "     ",
                "XXXXX",
                "     ",
                "     ",
                "     "
            });

            _characters['_'] = new BitmapSprite(new[] {
                "     ",
                "     ",
                "     ",
                "     ",
                "     ",
                "     ",
                "XXXXX"
            });

            _characters['+'] = new BitmapSprite(new[] {
                "     ",
                "  X  ",
                "  X  ",
                "XXXXX",
                "  X  ",
                "  X  ",
                "     "
            });

            _characters['*'] = new BitmapSprite(new[] {
                "     ",
                "X   X",
                " X X ",
                "XXXXX",
                " X X ",
                "X   X",
                "     "
            });

            _characters['.'] = new BitmapSprite(new[] {
                "     ",
                "     ",
                "     ",
                "     ",
                "     ",
                " XX  ",
                " XX  "
            });

            _characters['='] = new BitmapSprite(new[] {
                "     ",
                "     ",
                "XXXXX",
                "     ",
                "XXXXX",
                "     ",
                "     "
            });

            _characters['/'] = new BitmapSprite(new[] {
                "    X",
                "   X ",
                "   X ",
                "  X  ",
                " X   ",
                " X   ",
                "X    "
            });

            _characters['('] = new BitmapSprite(new[] {
                "  X  ",
                " X   ",
                "X    ",
                "X    ",
                "X    ",
                " X   ",
                "  X  "
            });

            _characters[')'] = new BitmapSprite(new[] {
                "  X  ",
                "   X ",
                "    X",
                "    X",
                "    X",
                "   X ",
                "  X  "
            });

            _characters['['] = new BitmapSprite(new[] {
                " XXX ",
                " X   ",
                " X   ",
                " X   ",
                " X   ",
                " X   ",
                " XXX "
            });

            _characters[']'] = new BitmapSprite(new[] {
                " XXX ",
                "   X ",
                "   X ",
                "   X ",
                "   X ",
                "   X ",
                " XXX "
            });
        }

        public void DrawText(Renderer renderer, string text, Vector2 position, Color color, float scale = 1.0f)
        {
            var x = position.X;
            var y = position.Y;

            foreach (char c in text.ToUpper())
            {
                if (_characters.TryGetValue(c, out var sprite))
                {
                    var scaledPos = new Vector2(x, y);
                    renderer.DrawSprite(sprite, scaledPos, color);
                }

                x += _characterWidth * scale + _spacing;
            }
        }

        public void DrawTextCentered(Renderer renderer, string text, float y, Color color, float scale = 1.0f)
        {
            var textWidth = MeasureString(text, scale);
            var x = (GameConstants.GAME_WIDTH - textWidth) / 2;
            DrawText(renderer, text, new Vector2(x, y), color, scale);
        }

        public void DrawTextCenteredInRect(Renderer renderer, string text, Rectangle bounds, Color color, float scale = 1.0f)
        {
            var textWidth = MeasureString(text, scale);
            var textHeight = _characterHeight * scale;

            var x = bounds.X + (bounds.Width - textWidth) / 2;
            var y = bounds.Y + (bounds.Height - textHeight) / 2;

            DrawText(renderer, text, new Vector2(x, y), color, scale);
        }

        public float MeasureString(string text, float scale = 1.0f)
        {
            return text.Length * (_characterWidth + _spacing) * scale - _spacing * scale;
        }
    }
}
