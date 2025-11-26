using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//왜 나눴지
namespace customSoundDownKey
{
    public class SoundFileService
    {

        private readonly string soundDirectory;

        // 생성자에서 'Sound' 폴더 경로 설정 및 폴더 존재 여부 확인
        public SoundFileService()
        {
            string exe = Assembly.GetExecutingAssembly().Location;
            string baseDir = Path.GetDirectoryName(exe);
            soundDirectory = Path.Combine(baseDir, "Sound");

            if (!Directory.Exists(soundDirectory))
            {
                MessageBox.Show($"'Sound' 폴더를 찾을 수 없습니다.\n폴더를 재생성합니다", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Directory.CreateDirectory(soundDirectory);
            }
        }



        // 'Sound' 폴더 내의 모든 파일 이름을 배열로 반환
        public string[] GetSoundList()
        {
            return Directory.GetFiles(soundDirectory)
                            .Select(Path.GetFileName)
                            .ToArray();
        }

        // 특정 파일 이름에 대한 전체 경로 반환
        public string GetFullPath(string fileName)
        {
            return Path.Combine(soundDirectory, fileName);
        }
    }
}
