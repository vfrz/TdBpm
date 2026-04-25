using SoundFlow.Backends.MiniAudio;
using SoundFlow.Midi.PortMidi;
using SoundFlow.Midi.Routing.Nodes;
using SoundFlow.Midi.Structs;

namespace SuperBpm;

public class BpmEngine : IDisposable
{
    public int Bpm
    {
        get;
        set => field = Math.Clamp(value, 30, 300);
    } = 120;
    
    public int RunningBpm { get; private set; } = 120;

    private readonly MiniAudioEngine _miniAudioEngine;

    private MidiOutputNode? _outputNode;

    private readonly HighResolutionTimer _timer = new();

    private static readonly MidiMessage ClockMessage = new(0xF8, 0, 0);

    public bool Running => _timer.IsRunning;

    public BpmEngine()
    {
        _miniAudioEngine = new MiniAudioEngine();

        _timer.Elapsed += (_, _) => { _outputNode!.ProcessMessage(ClockMessage); };
    }

    public void Initialize()
    {
        _miniAudioEngine.UsePortMidi();
        _miniAudioEngine.UpdateMidiDevicesInfo();
    }

    public void SelectMidiDevice(string name)
    {
        var outputDeviceInfo = _miniAudioEngine.MidiOutputDevices
            .SingleOrDefault(device => device.Name == name);

        if (outputDeviceInfo.Name == null)
        {
            throw new Exception("No MIDI output device found. Exiting.");
        }

        Console.WriteLine($"Found output device: '{outputDeviceInfo.Name}'. Attempting to send a note...");

        _outputNode = _miniAudioEngine.MidiManager.GetOrCreateOutputNode(outputDeviceInfo);
    }

    public void Restart()
    {
        if (_timer.IsRunning)
        {
            _timer.Stop();
            var stopMessage = new MidiMessage(0xFC, 0, 0);
            _outputNode!.ProcessMessage(stopMessage);
        }

        _timer.Interval = 60000 / ((float) Bpm * 24);
        RunningBpm = Bpm;

        var startMessage = new MidiMessage(0xFA, 0, 0);
        _outputNode!.ProcessMessage(startMessage);

        _timer.Start();
    }

    public void Stop()
    {
        if (!_timer.IsRunning)
            return;

        _timer.Stop();

        var stopMessage = new MidiMessage(0xFC, 0, 0);
        _outputNode!.ProcessMessage(stopMessage);
    }

    public void Dispose()
    {
        _miniAudioEngine.Dispose();
    }
}