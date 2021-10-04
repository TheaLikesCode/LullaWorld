
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LullaWorld
{
    /**
   * Thea Marie Alnæs
   * Programmering 3 prosjekt
   * 30.05.2014
   */
    interface IMyDrawable
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void LoadContent(Game game);

    }
}
