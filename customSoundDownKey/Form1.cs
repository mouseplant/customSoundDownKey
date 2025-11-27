// 참고: NAudio 라이브러리가 필요합니다. NuGet 패키지 매니저에서 NAudio를 설치하세요.
// 참고: Gma.System.MouseKeyHook 라이브러리가 필요합니다. NuGet 패키지 매니저에서 설치하세요.
// attention: this code uses NAudio and Gma.System.MouseKeyHook libraries.
// Make sure to install them via NuGet Package Manager.
// 오.. ai가 주석도 달아주네 
using Gma.System.MouseKeyHook;
using System.Runtime.InteropServices;

namespace customSoundDownKey
{
    public partial class Form1 : Form
    {
        private IKeyboardMouseEvents m_GlobalHook;
        private bool[] downKey = new bool[256];

        //만든 클래스 
        private SoundFileService soundFileService = new SoundFileService();
        private SoundManager soundManager = new SoundManager(); 

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
            // Sound 폴더내 음악파일 리스트 박스에 올리기
            LoadSoundFolderContentsToListBox();

            // 전역 키보드 후킹 시작
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyDown += GlobalHook_KeyDown;
            m_GlobalHook.KeyUp += GlobalHook_KeyUp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- 키 다운 이벤트 핸들러 (Async 사용) ---
        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            if(!downKey[e.KeyValue])
            {
                downKey[e.KeyValue] = true;
                soundManager.Play();
            }
            
        }

        private void GlobalHook_KeyUp(object sender, KeyEventArgs e)
        {
            downKey[e.KeyValue] = false;
        }


        // --- 종료 시 자원 해제 (필수) ---
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_GlobalHook != null)
            {
                m_GlobalHook.KeyDown -= GlobalHook_KeyDown;
                m_GlobalHook.KeyUp -= GlobalHook_KeyUp;
                m_GlobalHook.Dispose(); // 전역 후킹 반드시 해제
            }
        }


        // --- volume scroll event handler ---
        // --- 음량 조절 트랙바 이벤트 핸들러 ---
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            soundManager.SetVolume(VolumeBar.Value);
            Volume_label.Text = "음량: " + VolumeBar.Value;
        }

        // --- 적용 button event handler ---
        // --- 적용 버튼 이벤트 핸들러 ---
        private void button3_Click(object sender, EventArgs e)
        {
            // 박스 골라진게 없을경우 예외처리
            if (SoundList_Box.SelectedItem == null)
            {
                MessageBox.Show("파일을 선택하지 않았습니다.", "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 사운드 매니저에 선택된 사운드 파일 경로 설정
            soundManager.SetSoundPath(soundFileService.GetFullPath(SoundList_Box
                .SelectedItem
                .ToString()));
            Apply_Button.Enabled = false;
           
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
                if(itemText == soundManager.GetSoundFileName())
                {
                    Apply_Button.Enabled = false;
                    return;
                }
                Apply_Button.Enabled = true;
                //MessageBox.Show 메서드를 사용하여 항목의 텍스트를 사용자에게 보여줍니다.
                //MessageBox.Show($"선택된 항목: {itemText}", "ListBox 항목 선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Apply_Button.Enabled = false;
            }
        }

        private void LoadSoundFolderContentsToListBox()
        {
            // ListBox 초기화
            SoundList_Box.Items.Clear();

            try
            {
                string[] soundlist = soundFileService.GetSoundList();
                foreach (string s in soundlist)
                {
                    SoundList_Box.Items.Add(s);
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