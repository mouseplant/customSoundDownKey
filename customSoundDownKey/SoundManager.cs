using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace customSoundDownKey
{
    public class SoundManager
    {
        private string currentPath;
        private int volume = 25;
        public SoundManager()
        {
            currentPath = string.Empty;
        }
        public SoundManager(string path)
        {
            currentPath = path;
        }
        public void SetSoundPath(string path)
        {
            currentPath = path;
        }

        public string GetSoundFileName()
        {
            if (string.IsNullOrEmpty(currentPath))
                return string.Empty;
            return Path.GetFileName(currentPath);
        }
        public string GetSoundPath()
        {
            return currentPath;
        }

        public void SetVolume(int vol)
        {
            volume = vol;
        }

        public void Play()
        {

            Task.Run(() =>
            {
                if (!File.Exists(currentPath))
                {
                    // MessageBox.Show는 UI 스레드에서만 가능하므로 Console.WriteLine으로 대체
                    Console.WriteLine($"오류: 사운드 파일 경로를 찾을 수 없습니다: {currentPath}");
                    return;
                }

                AudioFileReader audioFile = null;
                // WaveOutEvent 대신 WasapiOut을 사용하여 낮은 레이턴시를 달성
                IWavePlayer outputDevice = null;

                try
                {
                    // 1. 새로운 AudioFileReader 객체 생성: 매번 처음부터 재생 가능
                    audioFile = new AudioFileReader(currentPath);

                    // 1-1. audioFile에 volume 변수에 맞게 사운드를 조절할수 있게 한다
                    audioFile.Volume = 1.0f * ((float)volume / 25);

                    // 2. WasapiOut 초기화: 저 레이턴시 설정
                    // AudioClientShareMode.Shared: 다른 앱과 공유
                    // latency: 밀리초 단위, 50ms 이하는 매우 낮음 (하드웨어/드라이버에 따라 제한될 수 있음)
                    outputDevice = new WasapiOut(AudioClientShareMode.Shared, false, 50);

                    outputDevice.Init(audioFile);

                    // 3. 재생 완료 시 객체 해제 로직
                    // 객체 생명주기가 이 함수 내에서 끝나므로, 재생 완료 후 즉시 자원을 해제해야 합니다.
                    outputDevice.PlaybackStopped += (s, ev) =>
                    {
                        // outputDevice와 audioFile 객체 해제 (Dispose)
                        outputDevice?.Dispose();
                        audioFile?.Dispose();
                    };

                    outputDevice.Play();
                }
                catch (Exception ex)
                {
                    // 재생 중 예외 처리 및 자원 정리
                    Console.WriteLine($"사운드 재생 오류: {ex.Message}");
                    outputDevice?.Dispose();
                    audioFile?.Dispose();
                }

                // 참고: 최대 10개까지 겹쳐 재생하는 기능은 이 구조 자체가 독립적인 재생 세션을
                // 키 다운 이벤트마다 생성하므로 자연스럽게 구현됩니다.
            });
        }
    }
}
