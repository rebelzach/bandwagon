namespace Bandwagon.Web.Services;

public interface IPlayback
{
    void Pause();
    void Play();
    void SetPosition(double positionSeconds);
}
