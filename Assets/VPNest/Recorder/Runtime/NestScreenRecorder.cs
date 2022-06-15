#if UNITY_EDITOR

using System.IO;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Events;

namespace VP.Nest.System.NestScreenRecorder
{
    public class NestScreenRecorder : Singleton<NestScreenRecorder>
    {
        [Header("Settings")] 
        [SerializeField] private RecorderData recorderData;
        [SerializeField] private bool captureUI = true;

        [Header("Video Settings")] 
        [SerializeField,Tooltip("Select MOV format for higher video quality")] 
        private VideoOutputFormat targetVideoOutputFormat = VideoOutputFormat.MP4;
        [SerializeField] private VideoQuality targetVideoQuality = VideoQuality.Low;
        [SerializeField,Tooltip("Start video recording as soon as game starts.")] 
        private bool autoStartVideoRecording = false;
        
        public enum VideoQuality
        {
            Low,
            Medium,
            High
        }
        
        public enum VideoOutputFormat
        {
            MP4,
            MOV
        }
    
        private RecorderController recorderController;
        private RecorderControllerSettings recorderControllerSettings;
        private ImageRecorderSettings imageRecorderSettings;
        private MovieRecorderSettings movieRecorderSettings;
        private string imageOutputFolder;
        private string videoOutputFolder;
        private string lastRecordedImageName = "null_image";

        private bool isImageRecording = false;
        private bool isVideoRecording = false;
        public bool IsRecording { get; private set; }
        public bool IsDeviceSimulatorRunning { get; private set; }

        public UnityAction OnRecordingStateChanged;

        private void Reset()
        {
            if (Resources.Load<RecorderData>("RecorderData")!= null)
            {
                recorderData = Resources.Load<RecorderData>("RecorderData");
            }
            else
            {
                Debug.LogError($"{GetType().Name} -> RecorderData.asset file not found in Resources folder!");
            }           
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            IsRecording = false;
        }

        private void OnEnable()
        {
            OnRecordingStateChanged += CheckRecordingState;
        }

        private void Start()
        {
            CheckDeviceSimulatorState();
            
            if (autoStartVideoRecording == true)
            {
                if (IsDeviceSimulatorRunning == false)
                {
                    TakeVideoCapture();
                }
                else
                {
                    Debug.LogError($"{GetType().Name} -> Unable autostart video recording. Please switch to Gameview. Recorder can not work with Simulator.");
                }
            }
        }

        private void Update()
        {
            CheckDeviceSimulatorState(); //TODO: might not be required to check every frame

            if (IsDeviceSimulatorRunning == false)
            {
                if ((Input.GetKeyDown(KeyCode.Alpha0) || (Input.GetKeyDown(KeyCode.Keypad0))) && isImageRecording == false && isVideoRecording == false)
                {
                    StartCoroutine(TakeAllScreenShotsAsync());
                }
                else if ((Input.GetKeyDown(KeyCode.Alpha0) || (Input.GetKeyDown(KeyCode.Keypad0))) && isImageRecording == true)
                {
                    Debug.LogError($"{GetType().Name} -> Recorder is busy! Please wait until recording process completed.");
                }

                if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
                {
                    TakeVideoCapture();
                }
            }
            else
            {
                Debug.LogError($"{GetType().Name} -> Please switch to Gameview. Recorder can not work with Simulator.");
            }
        }

        private void OnDisable()
        {
            OnRecordingStateChanged -= CheckRecordingState;
        }

        [MenuItem("Nest/VP Nest Screen Recorder/Create Recorder Instance", false)]
        public static void CreateNestScreenRecorder()
        {
            if (FindObjectOfType<NestScreenRecorder>() == null)
            {
                GameObject nestRecorder = new GameObject("NestScreenRecorder", typeof(NestScreenRecorder));
            }
            else
            {
                Debug.LogError($"NestScreenRecorder -> Recorder is already exist in hierarcy!");
            }
        }

        #region Video Recording

        private void InitializeVideoRecorderController()
        {
            recorderControllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
            recorderController = new RecorderController(recorderControllerSettings);

            videoOutputFolder = Path.Combine(Application.dataPath, "..", "Recordings/Videos");

            movieRecorderSettings = ScriptableObject.CreateInstance<MovieRecorderSettings>();
            movieRecorderSettings.name = "Video Recorder";
            movieRecorderSettings.Enabled = true;
            //movieRecorderSettings.Take = GetVideoFileCount();
            movieRecorderSettings.Take = recorderData.videoTakeCount;

            // This performs a MP4 recording
            if (targetVideoOutputFormat == VideoOutputFormat.MP4)
            {
                movieRecorderSettings.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
            }
            else if (targetVideoOutputFormat == VideoOutputFormat.MOV)
            {
                movieRecorderSettings.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MOV;
            }
            
            movieRecorderSettings.VideoBitRateMode = GetTargetVideoBitrateMode(movieRecorderSettings);

            movieRecorderSettings.ImageInputSettings = new GameViewInputSettings
            {
                OutputWidth = 1280,
                OutputHeight = 1600
            };

            movieRecorderSettings.AudioInputSettings.PreserveAudio = false;
            
            movieRecorderSettings.OutputFile = videoOutputFolder + "/" + $"{DefaultWildcard.Project}_{DefaultWildcard.Resolution}_{DefaultWildcard.Take}";
            
            // movieRecorderSettings.OutputFile = Path.Combine(videoOutputFolder,
            //     $"{DefaultWildcard.Project}_{DefaultWildcard.Resolution}_{DefaultWildcard.Take}"); ;

            // Setup Recording
            recorderControllerSettings.AddRecorderSettings(movieRecorderSettings);
            recorderControllerSettings.SetRecordModeToManual();
            recorderControllerSettings.FrameRate = 30.0f;
            SetUIVisibility(captureUI); //Decide should be hidden or not

            RecorderOptions.VerboseMode = false;
            recorderController.PrepareRecording();
            recorderController.StartRecording();
            isVideoRecording = true;
            recorderData.videoTakeCount += 1;
#if UNITY_EDITOR
            EditorUtility.SetDirty(recorderData);
#endif
            OnRecordingStateChanged.Invoke();

            Debug.Log($"{GetType().Name} -> Video recording started!");
        }

        private void TakeVideoCapture()
        {
            if (isImageRecording == false && isVideoRecording == false)
            {
                InitializeVideoRecorderController();
            }
            else
            {
                recorderController.StopRecording();
                SetUIVisibility(true); //Show UI
                isVideoRecording = false;
                OnRecordingStateChanged.Invoke();

                Debug.Log($"{GetType().Name} -> Video recording finished!");
            }
        }

        private int GetVideoFileCount()
        {
            var directoryInfo = new DirectoryInfo(videoOutputFolder);
            int fileCount;

            if (!Directory.Exists(videoOutputFolder))
            {
                fileCount = 0;
            }
            else
            {
                var files = directoryInfo.GetFiles("*.mp4");
                fileCount = files.Length;
            }

            return fileCount;
        }

        #endregion

        #region Image Recording

        private IEnumerator TakeAllScreenShotsAsync()
        {
            isImageRecording = true;
            OnRecordingStateChanged.Invoke();

            SetUIVisibility(captureUI); //Decide should be hidden or not

            SetImageRecorderSettings(1242, 2208);
            yield return StartCoroutine(TakeScreenshot());

            SetImageRecorderSettings(1242, 2688);
            yield return StartCoroutine(TakeScreenshot());

            SetImageRecorderSettings(2048, 2732);
            yield return StartCoroutine(TakeScreenshot());

            SetUIVisibility(true); //make sure ui is visible after screenshots taken

            recorderData.screenshotTakeCount += 1;
#if UNITY_EDITOR
            EditorUtility.SetDirty(recorderData);
#endif


            isImageRecording = false;
            OnRecordingStateChanged.Invoke();
        }

        private void InitializeRecorderController()
        {
            recorderControllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
            recorderController = new RecorderController(recorderControllerSettings);

            imageOutputFolder = Path.Combine(Application.dataPath, "..", "Recordings/Screenshots");
        }

        private void SetImageRecorderSettings(int imageWidth, int imageHeight)
        {
            InitializeRecorderController();

            imageRecorderSettings = ScriptableObject.CreateInstance<ImageRecorderSettings>();
            imageRecorderSettings.name = "Image Recorder";
            imageRecorderSettings.Enabled = true;
            imageRecorderSettings.OutputFormat = ImageRecorderSettings.ImageRecorderOutputFormat.JPEG;
            imageRecorderSettings.CaptureAlpha = false;
            //imageRecorderSettings.Take = GetImageFileCount() / 3;
            imageRecorderSettings.Take = recorderData.screenshotTakeCount;

            imageRecorderSettings.OutputFile = imageOutputFolder+ "/" + $"{DefaultWildcard.Project}_{DefaultWildcard.Resolution}_{DefaultWildcard.Take}";
            
            // imageRecorderSettings.OutputFile = Path.Combine(imageOutputFolder,
            //     $"{DefaultWildcard.Project}_{DefaultWildcard.Resolution}_{DefaultWildcard.Take}");

            imageRecorderSettings.imageInputSettings = new GameViewInputSettings
            {
                OutputWidth = imageWidth,
                OutputHeight = imageHeight,
            };

            lastRecordedImageName = $"{imageWidth}x{imageHeight}_{imageRecorderSettings.Take}.{imageRecorderSettings.OutputFormat}";
        }

        private IEnumerator TakeScreenshot()
        {
            recorderControllerSettings.RemoveRecorder(imageRecorderSettings); //null?
            recorderControllerSettings.AddRecorderSettings(imageRecorderSettings);
            recorderControllerSettings.SetRecordModeToSingleFrame(0);

            recorderController.PrepareRecording();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            recorderController.StartRecording();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            

            Debug.Log($"{GetType().Name} -> {lastRecordedImageName}");
        }

        private int GetImageFileCount()
        {
            var directoryInfo = new DirectoryInfo(imageOutputFolder);
            int fileCount;

            if (!Directory.Exists(imageOutputFolder))
            {
                fileCount = 0;
            }
            else
            {
                var files = directoryInfo.GetFiles("*.jpg");
                fileCount = files.Length;
            }

            return fileCount;
        }

        #endregion

        private void SetUIVisibility(bool isEnabled)
        {
            var canvasList = FindObjectsOfType<Canvas>();

            foreach (var canvas in canvasList)
            {
                //canvas.enabled = isEnabled;

                if (canvas.gameObject.GetComponent<CanvasGroup>() == null)
                {
                    canvas.gameObject.AddComponent<CanvasGroup>();
                }

                if (isEnabled)
                {
                    canvas.gameObject.GetComponent<CanvasGroup>().alpha = 1;
                }
                else
                {
                    canvas.gameObject.GetComponent<CanvasGroup>().alpha = 0;
                }
            }
        }

        private VideoBitrateMode GetTargetVideoBitrateMode(MovieRecorderSettings movieRecorderSettings)
        {
            if (targetVideoQuality == VideoQuality.Low)
            {
                return VideoBitrateMode.Low;
            }
            else if (targetVideoQuality == VideoQuality.Medium)
            {
                return VideoBitrateMode.Medium;
            }
            else if (targetVideoQuality == VideoQuality.High)
            {
                return VideoBitrateMode.High;
            }
            else
            {
                Debug.Log($"{GetType().Name} -> Unable to detect target video quality, 'Low' settled as default.");
                return VideoBitrateMode.Low;
            }
        }
        
        private void CheckRecordingState()
        {
            if (isImageRecording == true || isVideoRecording == true)
            {
                IsRecording = true;
                //Debug.Log($"{GetType().Name} -> IsRecording {IsRecording} ");
            }
            else if (isImageRecording == false && isVideoRecording == false)
            {
                IsRecording = false;
                //Debug.Log($"{GetType().Name} -> IsRecording {IsRecording} ");
            }
        }

        private void CheckDeviceSimulatorState()
        {
            if (Application.isPlaying == true)
            {
                //IsDeviceSimulatorRunning = Application.isMobilePlatform ? true : false;
                if (Application.isMobilePlatform == true 
                    && EditorWindow.focusedWindow != null
                    && EditorWindow.focusedWindow.ToString() == " (Unity.DeviceSimulator.SimulatorWindow)")
                {
                    IsDeviceSimulatorRunning = true;
                }
                else
                {
                    IsDeviceSimulatorRunning = false; 
                }
            }
        }
    }
}

#endif
