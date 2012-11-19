A SpriteFont style Bordered Font renderer for Xna.

Usage:

Add a reference to IronXna in your code project and a reference to IronXna.Design in your content projects.
In your content project, add a new SpriteFont.
Edit the Spritefont and add the following:

    <!--
    Thickness of the border to generate. In Pixels
    -->
    <BorderThickness>12</BorderThickness>
    <!--
    Whether to include a double sized font for use with retina devices. If included and a (iOS) retina device is detected we will draw at the same size but with a higher quality texture
    -->
    <IncludeRetina>false</IncludeRetina>

Choose the spritefont in the solution explorer and change the "Content Importer" and "Content Processor" to "Bordered Font Importer" / "Bordered Text Processor" (TODO: Rename these to be the same)

Now load it with:
_timesNewRomanBordered = Content.Load<BorderedFont>("TimesNewRomanBordered");

And draw with the extension methods from SpriteBatchBorderedFontExtensions:
_spriteBatch.DrawString(_timesNewRomanBordered, "Hi", new Vector2(100, 450), Color.Black, Color.White);


License: Do whatever you want with this code :)
