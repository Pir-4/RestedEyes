﻿namespace RestedEyes
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.labelCurrentTime = new System.Windows.Forms.Label();
            this.labelSpendRestTime = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.labelSpendWorkTime = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelRestTime = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.svaeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.labelCurrentTime.AutoSize = true;
            this.labelCurrentTime.Location = new System.Drawing.Point(12, 35);
            this.labelCurrentTime.Name = "label1";
            this.labelCurrentTime.Size = new System.Drawing.Size(87, 13);
            this.labelCurrentTime.TabIndex = 1;
            this.labelCurrentTime.Text = "Текущее время";
            // 
            // label2
            // 
            this.labelSpendRestTime.AutoSize = true;
            this.labelSpendRestTime.Location = new System.Drawing.Point(128, 70);
            this.labelSpendRestTime.Name = "label2";
            this.labelSpendRestTime.Size = new System.Drawing.Size(80, 13);
            this.labelSpendRestTime.TabIndex = 2;
            this.labelSpendRestTime.Text = "Отдхы прошло";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(206, 94);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(54, 21);
            this.button2.TabIndex = 3;
            this.button2.Text = "Отдых";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.labelSpendWorkTime.AutoSize = true;
            this.labelSpendWorkTime.Location = new System.Drawing.Point(13, 70);
            this.labelSpendWorkTime.Name = "label3";
            this.labelSpendWorkTime.Size = new System.Drawing.Size(86, 13);
            this.labelSpendWorkTime.TabIndex = 4;
            this.labelSpendWorkTime.Text = "Работы прошло";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(128, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Время начала";
            // 
            // label5
            // 
            this.labelRestTime.AutoSize = true;
            this.labelRestTime.Location = new System.Drawing.Point(12, 98);
            this.labelRestTime.Name = "label5";
            this.labelRestTime.Size = new System.Drawing.Size(79, 13);
            this.labelRestTime.TabIndex = 6;
            this.labelRestTime.Text = "Время отдыха";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(122, 98);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Совещание";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem3});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(260, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.svaeToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolStripTextBox1});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(68, 20);
            this.toolStripMenuItem1.Text = "Конфинг";
            // 
            // svaeToolStripMenuItem
            // 
            this.svaeToolStripMenuItem.Name = "svaeToolStripMenuItem";
            this.svaeToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.svaeToolStripMenuItem.Text = "Сохранить как";
            this.svaeToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(153, 22);
            this.toolStripMenuItem2.Text = "Сохранить";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(153, 22);
            this.toolStripTextBox1.Text = "Открыть";
            this.toolStripTextBox1.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripComboBox1});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(91, 20);
            this.toolStripMenuItem3.Text = "Автозагрузка";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem4.Text = "Добавить";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.AutoloadToolStripMenuItem_Click);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.AutoSize = false;
            this.toolStripComboBox1.IntegralHeight = false;
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(80, 23);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 131);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.labelRestTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelSpendWorkTime);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.labelSpendRestTime);
            this.Controls.Add(this.labelCurrentTime);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "ResteEyes";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelCurrentTime;
        private System.Windows.Forms.Label labelSpendRestTime;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label labelSpendWorkTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelRestTime;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem svaeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
    }
}

