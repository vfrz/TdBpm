using Raylib_cs;

namespace SuperBpm;

public class SuperBpmApp : IDisposable
{
    private readonly BpmEngine _bpmEngine = new();

    public void Run()
    {
        _bpmEngine.Initialize();
        _bpmEngine.SelectMidiDevice("TD-3 MIDI 1");

        Raylib.InitWindow(800, 480, "SuperBpm");

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText("SuperBPM", 8, 8, 72, Color.Orange);

            Raylib.DrawText("Current BPM:", 8, 96, 60, Color.White);

            var color = _bpmEngine.Bpm == _bpmEngine.RunningBpm ? Color.Green : Color.Red;
            Raylib.DrawText(_bpmEngine.Bpm.ToString(), 442, 96, 60, color);

            if (Raylib.IsKeyPressed(KeyboardKey.S))
            {
                _bpmEngine.Stop();
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Enter))
            {
                _bpmEngine.Restart();
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Up) || Raylib.IsKeyPressedRepeat(KeyboardKey.Up))
            {
                _bpmEngine.Bpm++;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Down) || Raylib.IsKeyPressedRepeat(KeyboardKey.Down))
            {
                _bpmEngine.Bpm--;
            }

            Raylib.EndDrawing();
        }

        _bpmEngine.Stop();
        
        Raylib.CloseWindow();
    }

    public void Dispose()
    {
        _bpmEngine.Dispose();
    }
}