using System.Timers; // Timers 命名空间

public class SComponet {
    public SimObject gameObject;
    public virtual void Start(){}
    public virtual void Update(){}

    Timer t;

    // 驱动子类定义的Start方法
    public void DriveStart(SimObject obj){
        gameObject = obj;
        Start();

        t = new Timer();
        t.Interval = 10;//定时周期10毫秒
        t.Elapsed += new ElapsedEventHandler(DriveUpdata);
        t.Enabled = true;
        t.Start();
    }

    // 驱动子类定义的Update方法
    public void DriveUpdata(object source, ElapsedEventArgs e){
        Update();
    }

    // 停止Timers，清除Timers对象，这很重要，容易引起内存泄露
    public void Disposed(){
        t.Stop();
        t = null;
    }
}