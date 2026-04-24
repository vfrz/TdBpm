using Raylib_cs;

namespace TdBpm;

public class BpmApplication
{
    private readonly BpmEngine _bpmEngine = new();

    public void Run()
    {
        _bpmEngine.Initialize();
        _bpmEngine.SelectMidiDevice("TD-3 MIDI 1");

        Raylib.InitWindow(800, 480, "TdBpm");

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Raylib.DrawText($"Current BPM: {_bpmEngine.Bpm}", 12, 12, 60, Color.Orange);

            if (Raylib.IsKeyPressed(KeyboardKey.S))
            {
                if (_bpmEngine.Running)
                {
                    _bpmEngine.Stop();
                }
                else
                {
                    _bpmEngine.Restart();
                }
            }

            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                _bpmEngine.Restart();
            }

            if (Raylib.IsKeyPressed(KeyboardKey.E) || Raylib.IsKeyPressedRepeat(KeyboardKey.E))
            {
                _bpmEngine.Bpm++;
            }
            
            if (Raylib.IsKeyPressed(KeyboardKey.D) || Raylib.IsKeyPressedRepeat(KeyboardKey.D))
            {
                _bpmEngine.Bpm--;
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}