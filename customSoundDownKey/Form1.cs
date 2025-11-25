using NAudio.Wave;
using NAudio.CoreAudioApi; // WasapiOut 사용을 위해 추가
using Gma.System.MouseKeyHook;
using System.Reflection;
using System.Runtime.InteropServices;


namespace customSoundDownKey
{
    public partial class Form1 : Form
    {
        private IKeyboardMouseEvents m_GlobalHook;
        private string soundFilePath = "";
        //private readonly string soundFilePath = Path.Combine(Application.StartupPath, "sound.mp3");

        // 최적화 핵심 1: 사운드 파일을 메모리에 한 번만 로드할 버퍼
        private byte[] audioBuffer;
        private WaveFormat waveFormat;
        private int volume = 25;
    
        // 디버깅을 위한 함수 =================
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();
        //====================================

        public Form1()
        {
            //AllocConsole();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            LoadSoundFolderContentsToListBox();

            // --- 기존에 Form1_Load에 있던 Audio 객체 초기화 제거 ---
            // WASAPI/DirectSound 객체는 PlaySoundEffect 내에서 매번 새로 생성합니다.

            // 전역 키보드 후킹 시작
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyDown += GlobalHook_KeyDown;

            //LoadSoundBuffer(soundFilePath);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- 사운드 버퍼 로드 함수 ---
        private void LoadSoundBuffer(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"사운드 파일 경로를 찾을 수 없습니다: {filePath}", "오류");
                LoadSoundFolderContentsToListBox();
                return;
            }

            try
            {
                // AudioFileReader를 사용하여 전체 파일을 메모리로 읽어옵니다.
                using (var reader = new AudioFileReader(filePath))
                {
                    // WaveFormat 저장 (재생에 필요)
                    waveFormat = reader.WaveFormat;

                    // 파일 전체 크기만큼 메모리 버퍼 할당
                    audioBuffer = new byte[reader.Length];

                    // 버퍼에 데이터 읽기
                    reader.Read(audioBuffer, 0, audioBuffer.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"사운드 버퍼 로드 오류: {ex.Message}", "오류");
            }
        }

        // --- 키 다운 이벤트 핸들러 (Async 사용) ---
        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {

            // Task.Run을 사용하여 메인 스레드와 키 후킹 스레드를 막지 않고 
            // 별도의 스레드에서 사운드 재생을 비동기적으로 처리합니다.
            Task.Run(() => PlaySoundEffect(soundFilePath));

            // 이벤트를 다른 프로그램에 전달하지 않으려면 e.Handled = true; 를 추가
            // e.Handled = true; 
        }

        // --- 사운드 재생 로직 (다중 재생 및 저 레이턴시) ---
        private void PlaySoundEffect(string filePath)
        {
            if (!File.Exists(filePath))
            {
                // MessageBox.Show는 UI 스레드에서만 가능하므로 Console.WriteLine으로 대체
                Console.WriteLine($"오류: 사운드 파일 경로를 찾을 수 없습니다: {filePath}");
                return;
            }

            AudioFileReader audioFile = null;
            // WaveOutEvent 대신 WasapiOut을 사용하여 낮은 레이턴시를 달성
            IWavePlayer outputDevice = null;

            try
            {
                // 1. 새로운 AudioFileReader 객체 생성: 매번 처음부터 재생 가능
                audioFile = new AudioFileReader(filePath);

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
        }

        // --- 종료 시 자원 해제 (필수) ---
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_GlobalHook != null)
            {
                m_GlobalHook.KeyDown -= GlobalHook_KeyDown;
                m_GlobalHook.Dispose(); // 전역 후킹 반드시 해제
            }
        }

        private void volumeSlider1_Load(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            volume = VolumeBar.Value;
            Volume_label.Text = "음량: " + volume;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(SoundList_Box.SelectedItem == null)
            {
                MessageBox.Show("파일을 선택하지 않았습니다.", "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string exePath = Assembly.GetExecutingAssembly().Location;
            string baseDirectory = Path.GetDirectoryName(exePath);
            //MessageBox.Show(Path.Combine(Application.StartupPath, baseDirectory + "/Sound/" + SoundList_Box.SelectedItem.ToString()));
            soundFilePath = Path.Combine(Application.StartupPath, baseDirectory + "/Sound/" + SoundList_Box.SelectedItem.ToString());
            LoadSoundBuffer(soundFilePath);
        }

        private void SoundList_Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 1. 선택된 항목 가져오기
            // ListBox.SelectedItem 속성은 현재 선택된 항목의 값을 반환합니다. 
            // 이 값은 일반적으로 object 타입이므로, 문자열로 변환해야 합니다.
            object selectedItem = SoundList_Box.SelectedItem;

            // 선택된 항목이 null이 아닌지 확인 (아무것도 선택되지 않았거나 선택이 해제된 경우)
            if (selectedItem != null)
            {
                // 2. 메시지 박스로 표시
                string itemText = selectedItem.ToString();

                //MessageBox.Show 메서드를 사용하여 항목의 텍스트를 사용자에게 보여줍니다.

                MessageBox.Show($"선택된 항목: {itemText}", "ListBox 항목 선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoadSoundFolderContentsToListBox()
        {
            // ListBox 초기화
            SoundList_Box.Items.Clear();

            try
            {
                // 1. 현재 실행 중인 .exe 파일의 디렉토리 경로 가져오기
                string exePath = Assembly.GetExecutingAssembly().Location;
                string baseDirectory = Path.GetDirectoryName(exePath);

                if (baseDirectory == null)
                {
                    MessageBox.Show("프로그램 실행 경로를 찾을 수 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. 고정된 'Sound' 폴더의 전체 경로 설정
                // Path.Combine을 사용하여 운영체제에 맞는 경로 구분자(\ 또는 /)를 안전하게 사용합니다.
                string soundFolderPath = Path.Combine(baseDirectory, "Sound");

                // 3. 'Sound' 폴더가 실제로 존재하는지 확인
                if (!Directory.Exists(soundFolderPath))
                {
                    MessageBox.Show($"'Sound' 폴더를 찾을 수 없습니다.\n폴더를 재생성합니다", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Directory.CreateDirectory(soundFolderPath);
                    return;
                }

                //// 4. --- 'Sound' 폴더 내의 폴더 목록 가져와서 ListBox에 추가 ---
                //string[] directories = Directory.GetDirectories(soundFolderPath);

                //foreach (string dir in directories)
                //{
                //    string folderName = Path.GetFileName(dir);
                //    SoundList_Box.Items.Add($"[폴더] {folderName}");
                //}

                // 5. --- 'Sound' 폴더 내의 파일 목록 가져와서 ListBox에 추가 ---
                string[] files = Directory.GetFiles(soundFolderPath);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    SoundList_Box.Items.Add($"{fileName}");
                }

                // ListBox에 항목이 성공적으로 추가되었음을 확인
                if (SoundList_Box.Items.Count > 0)
                {
                    // MessageBox.Show($"'Sound' 폴더의 {ListBox1.Items.Count}개 항목을 로드했습니다.", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"'Sound' 폴더 내용 로딩 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Reload_Button_Click(object sender, EventArgs e)
        {
            LoadSoundFolderContentsToListBox();
        }
    }
}