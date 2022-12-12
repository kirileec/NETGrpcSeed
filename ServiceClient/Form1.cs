using Serilog;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace ServiceClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.Font = new System.Drawing.Font("Consolas", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        }

        private void init()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.RichTextBox(richTextBox1, theme: ThemePresets.Dark)
                 .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .Enrich.FromLogContext()
               .WriteTo.RollingFile("./logs/log-{Date}.log")
                .CreateLogger();

            Log.Information("Hello, world!");
        }
        //开启按扭
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;

            init();

        }
        //停止
        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
        }
        //查看任务列表
        private void button3_Click(object sender, EventArgs e)
        {
            var form = new ListTaskForm();
            form.Show();
        }
    }
}