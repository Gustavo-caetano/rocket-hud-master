using EasyUI.Toast;

class ToastUtil
{
    public static void ShowToastError(string msg)
    {
        Toast.Show(msg, 2f, ToastColor.Red, ToastPosition.TopCenter);
    }

    public static void ShowToastSucesss(string msg)
    {
        Toast.Show(msg, 2f, ToastColor.Green, ToastPosition.TopCenter);
    }
}