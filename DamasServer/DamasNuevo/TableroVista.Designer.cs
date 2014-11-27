namespace DamasNuevo
{
    partial class TableroVista
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableroVista));
            this.comm = new tcpServer.TcpServer(this.components);
            this.lblConnected = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // comm
            // 
            this.comm.Encoding = ((System.Text.Encoding)(resources.GetObject("comm.Encoding")));
            this.comm.IdleTime = 50;
            this.comm.IsOpen = false;
            this.comm.MaxCallbackThreads = 100;
            this.comm.MaxSendAttempts = 3;
            this.comm.Port = -1;
            this.comm.VerifyConnectionInterval = 100;
            this.comm.OnConnect += new tcpServer.tcpServerConnectionChanged(this.comm_OnConnect);
            this.comm.OnDataAvailable += new tcpServer.tcpServerConnectionChanged(this.comm_OnDataAvailable);
            // 
            // lblConnected
            // 
            this.lblConnected.AutoSize = true;
            this.lblConnected.Location = new System.Drawing.Point(12, 9);
            this.lblConnected.Name = "lblConnected";
            this.lblConnected.Size = new System.Drawing.Size(13, 13);
            this.lblConnected.TabIndex = 0;
            this.lblConnected.Text = "0";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(277, 2);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(100, 20);
            this.txtLog.TabIndex = 1;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // TableroVista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 365);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblConnected);
            this.Name = "TableroVista";
            this.Text = "Damas - Juego";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TableroVista_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private tcpServer.TcpServer comm;
        private System.Windows.Forms.Label lblConnected;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Timer timer;

    }
}

