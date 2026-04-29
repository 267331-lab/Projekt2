namespace NGOFinanceDashboard.Forms;

partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        
        // Main Container
        this.mainPanel = new System.Windows.Forms.TableLayoutPanel();
        
        // Input Panel
        this.inputPanel = new System.Windows.Forms.Panel();
        this.inputLayout = new System.Windows.Forms.FlowLayoutPanel();
        this.urlLabel = new System.Windows.Forms.Label();
        this.urlTextBox = new System.Windows.Forms.TextBox();
        this.fetchButton = new System.Windows.Forms.Button();
        this.progressLabel = new System.Windows.Forms.Label();
        
        // Results Panel
        this.resultsPanel = new System.Windows.Forms.Panel();
        this.resultsLayout = new System.Windows.Forms.TableLayoutPanel();
        this.cashFlowHeaderLabel = new System.Windows.Forms.Label();
        this.biggestExpenseHeaderLabel = new System.Windows.Forms.Label();
        this.topContributorsHeaderLabel = new System.Windows.Forms.Label();
        this.commonMessagesHeaderLabel = new System.Windows.Forms.Label();
        this.cashFlowValueLabel = new System.Windows.Forms.Label();
        this.biggestExpenseValueLabel = new System.Windows.Forms.Label();
        this.topContributorsValueLabel = new System.Windows.Forms.Label();
        this.commonMessagesValueLabel = new System.Windows.Forms.Label();
        
        // Grid Panel
        this.gridPanel = new System.Windows.Forms.Panel();
        this.gridTitleLabel = new System.Windows.Forms.Label();
        this.transactionsGrid = new System.Windows.Forms.DataGridView();

        // MainPanel
        this.mainPanel.ColumnCount = 1;
        this.mainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.mainPanel.Controls.Add(this.inputPanel, 0, 0);
        this.mainPanel.Controls.Add(this.resultsPanel, 0, 1);
        this.mainPanel.Controls.Add(this.gridPanel, 0, 2);
        this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.mainPanel.Padding = new System.Windows.Forms.Padding(10);
        this.mainPanel.RowCount = 3;
        this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
        this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 280F));
        this.mainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.mainPanel.BackColor = System.Drawing.Color.WhiteSmoke;

        // Input Panel
        this.inputPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.inputPanel.Controls.Add(this.inputLayout);
        this.inputPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.inputPanel.BackColor = System.Drawing.Color.White;

        // Input Layout
        this.inputLayout.AutoScroll = false;
        this.inputLayout.Controls.Add(this.urlLabel);
        this.inputLayout.Controls.Add(this.urlTextBox);
        this.inputLayout.Controls.Add(this.fetchButton);
        this.inputLayout.Controls.Add(this.progressLabel);
        this.inputLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        this.inputLayout.Padding = new System.Windows.Forms.Padding(10);

        // URL Label
        this.urlLabel.AutoSize = true;
        this.urlLabel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
        this.urlLabel.Margin = new System.Windows.Forms.Padding(5);
        this.urlLabel.Text = "Fio Bank Transparent Account URL:";

        // URL TextBox
        this.urlTextBox.Width = 700;
        this.urlTextBox.Text = "https://ib.fio.cz/ib/transparent?a=2200272480&f=27.04.2025&t=27.04.2026";
        this.urlTextBox.Margin = new System.Windows.Forms.Padding(5);

        // Fetch Button
        this.fetchButton.Text = "Fetch & Analyze";
        this.fetchButton.Width = 140;
        this.fetchButton.Height = 35;
        this.fetchButton.Margin = new System.Windows.Forms.Padding(5);
        this.fetchButton.BackColor = System.Drawing.Color.DodgerBlue;
        this.fetchButton.ForeColor = System.Drawing.Color.White;
        this.fetchButton.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
        this.fetchButton.Cursor = System.Windows.Forms.Cursors.Hand;

        // Progress Label
        this.progressLabel.AutoSize = true;
        this.progressLabel.Font = new System.Drawing.Font("Arial", 9F);
        this.progressLabel.ForeColor = System.Drawing.Color.Green;
        this.progressLabel.Margin = new System.Windows.Forms.Padding(5);
        this.progressLabel.Text = "Ready to load data";

        // Results Panel
        this.resultsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.resultsPanel.Controls.Add(this.resultsLayout);
        this.resultsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.resultsPanel.BackColor = System.Drawing.Color.White;

        // Results Layout
        this.resultsLayout.ColumnCount = 4;
        this.resultsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.resultsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.resultsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.resultsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
        this.resultsLayout.Controls.Add(this.cashFlowHeaderLabel, 0, 0);
        this.resultsLayout.Controls.Add(this.biggestExpenseHeaderLabel, 1, 0);
        this.resultsLayout.Controls.Add(this.topContributorsHeaderLabel, 2, 0);
        this.resultsLayout.Controls.Add(this.commonMessagesHeaderLabel, 3, 0);
        this.resultsLayout.Controls.Add(this.cashFlowValueLabel, 0, 1);
        this.resultsLayout.Controls.Add(this.biggestExpenseValueLabel, 1, 1);
        this.resultsLayout.Controls.Add(this.topContributorsValueLabel, 2, 1);
        this.resultsLayout.Controls.Add(this.commonMessagesValueLabel, 3, 1);
        this.resultsLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        this.resultsLayout.Padding = new System.Windows.Forms.Padding(15);
        this.resultsLayout.RowCount = 2;
        this.resultsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
        this.resultsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

        // Header Labels
        this.cashFlowHeaderLabel.AutoSize = false;
        this.cashFlowHeaderLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
        this.cashFlowHeaderLabel.ForeColor = System.Drawing.Color.DarkBlue;
        this.cashFlowHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.cashFlowHeaderLabel.Height = 35;
        this.cashFlowHeaderLabel.Dock = System.Windows.Forms.DockStyle.Top;
        this.cashFlowHeaderLabel.Text = "Total Cash Flow";

        this.biggestExpenseHeaderLabel.AutoSize = false;
        this.biggestExpenseHeaderLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
        this.biggestExpenseHeaderLabel.ForeColor = System.Drawing.Color.DarkBlue;
        this.biggestExpenseHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.biggestExpenseHeaderLabel.Height = 35;
        this.biggestExpenseHeaderLabel.Dock = System.Windows.Forms.DockStyle.Top;
        this.biggestExpenseHeaderLabel.Text = "Biggest Expense";

        this.topContributorsHeaderLabel.AutoSize = false;
        this.topContributorsHeaderLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
        this.topContributorsHeaderLabel.ForeColor = System.Drawing.Color.DarkBlue;
        this.topContributorsHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.topContributorsHeaderLabel.Height = 35;
        this.topContributorsHeaderLabel.Dock = System.Windows.Forms.DockStyle.Top;
        this.topContributorsHeaderLabel.Text = "Top 3 Contributors";

        this.commonMessagesHeaderLabel.AutoSize = false;
        this.commonMessagesHeaderLabel.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
        this.commonMessagesHeaderLabel.ForeColor = System.Drawing.Color.DarkBlue;
        this.commonMessagesHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.commonMessagesHeaderLabel.Height = 35;
        this.commonMessagesHeaderLabel.Dock = System.Windows.Forms.DockStyle.Top;
        this.commonMessagesHeaderLabel.Text = "Most Common Message";

        // Value Labels
        this.cashFlowValueLabel.AutoSize = false;
        this.cashFlowValueLabel.Font = new System.Drawing.Font("Arial", 10F);
        this.cashFlowValueLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
        this.cashFlowValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.cashFlowValueLabel.Padding = new System.Windows.Forms.Padding(5);
        this.cashFlowValueLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.cashFlowValueLabel.BackColor = System.Drawing.Color.AliceBlue;
        this.cashFlowValueLabel.Text = "—";

        this.biggestExpenseValueLabel.AutoSize = false;
        this.biggestExpenseValueLabel.Font = new System.Drawing.Font("Arial", 10F);
        this.biggestExpenseValueLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
        this.biggestExpenseValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.biggestExpenseValueLabel.Padding = new System.Windows.Forms.Padding(5);
        this.biggestExpenseValueLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.biggestExpenseValueLabel.BackColor = System.Drawing.Color.AliceBlue;
        this.biggestExpenseValueLabel.Text = "—";

        this.topContributorsValueLabel.AutoSize = false;
        this.topContributorsValueLabel.Font = new System.Drawing.Font("Arial", 10F);
        this.topContributorsValueLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
        this.topContributorsValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.topContributorsValueLabel.Padding = new System.Windows.Forms.Padding(5);
        this.topContributorsValueLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.topContributorsValueLabel.BackColor = System.Drawing.Color.AliceBlue;
        this.topContributorsValueLabel.Text = "—";

        this.commonMessagesValueLabel.AutoSize = false;
        this.commonMessagesValueLabel.Font = new System.Drawing.Font("Arial", 10F);
        this.commonMessagesValueLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
        this.commonMessagesValueLabel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.commonMessagesValueLabel.Padding = new System.Windows.Forms.Padding(5);
        this.commonMessagesValueLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.commonMessagesValueLabel.BackColor = System.Drawing.Color.AliceBlue;
        this.commonMessagesValueLabel.Text = "—  ";

        // Grid Panel
        this.gridPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.gridPanel.Controls.Add(this.transactionsGrid);
        this.gridPanel.Controls.Add(this.gridTitleLabel);
        this.gridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        this.gridPanel.BackColor = System.Drawing.Color.White;

        // Grid Title Label
        this.gridTitleLabel.Text = "All Transactions";
        this.gridTitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
        this.gridTitleLabel.Height = 25;
        this.gridTitleLabel.Padding = new System.Windows.Forms.Padding(10);
        this.gridTitleLabel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
        this.gridTitleLabel.BackColor = System.Drawing.Color.LightGray;

        // Transactions Grid
        this.transactionsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
        this.transactionsGrid.AllowUserToAddRows = false;
        this.transactionsGrid.ReadOnly = true;
        this.transactionsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.transactionsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
        this.transactionsGrid.BackgroundColor = System.Drawing.Color.White;
        this.transactionsGrid.AlternatingRowsDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = System.Drawing.Color.WhiteSmoke };
        this.transactionsGrid.Columns.Add("Date", "Date");
        this.transactionsGrid.Columns.Add("AccountName", "Account Name");
        this.transactionsGrid.Columns.Add("Amount", "Amount (CZK)");
        this.transactionsGrid.Columns.Add("Message", "Message");
        this.transactionsGrid.Columns["Date"].Width = 100;
        this.transactionsGrid.Columns["Amount"].Width = 120;

        // MainForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1400, 900);
        this.Controls.Add(this.mainPanel);
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "NGO Finance Dashboard - Fio Bank Transparent Account";
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel mainPanel;
    private System.Windows.Forms.Panel inputPanel;
    private System.Windows.Forms.FlowLayoutPanel inputLayout;
    private System.Windows.Forms.Label urlLabel;
    private System.Windows.Forms.TextBox urlTextBox;
    private System.Windows.Forms.Button fetchButton;
    private System.Windows.Forms.Label progressLabel;
    
    private System.Windows.Forms.Panel resultsPanel;
    private System.Windows.Forms.TableLayoutPanel resultsLayout;
    private System.Windows.Forms.Label cashFlowHeaderLabel;
    private System.Windows.Forms.Label biggestExpenseHeaderLabel;
    private System.Windows.Forms.Label topContributorsHeaderLabel;
    private System.Windows.Forms.Label commonMessagesHeaderLabel;
    private System.Windows.Forms.Label cashFlowValueLabel;
    private System.Windows.Forms.Label biggestExpenseValueLabel;
    private System.Windows.Forms.Label topContributorsValueLabel;
    private System.Windows.Forms.Label commonMessagesValueLabel;
    
    private System.Windows.Forms.Panel gridPanel;
    private System.Windows.Forms.Label gridTitleLabel;
    private System.Windows.Forms.DataGridView transactionsGrid;
}