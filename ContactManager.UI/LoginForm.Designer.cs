namespace ContactManager.UI;

partial class LoginForm
{
	private TableLayoutPanel LoginLayout = null!;
	private Label LoginTitle = null!;
	private Label UserNameLabel = null!;
	private TextBox UserNameInput = null!;
	private Label PasswordLabel = null!;
	private TextBox PasswordInput = null!;
	private Label LoginError = null!;
	private FlowLayoutPanel LoginActions = null!;
	private Button LoginButton = null!;
	private Button CancelLoginButton = null!;

	#region Windows Form Designer generated code

	private void InitializeComponent()
	{
		LoginLayout = new TableLayoutPanel();
		LoginTitle = new Label();
		UserNameLabel = new Label();
		UserNameInput = new TextBox();
		PasswordLabel = new Label();
		PasswordInput = new TextBox();
		LoginError = new Label();
		LoginActions = new FlowLayoutPanel();
		LoginButton = new Button();
		CancelLoginButton = new Button();
		LoginLayout.SuspendLayout();
		LoginActions.SuspendLayout();
		SuspendLayout();
		//
		// LoginLayout
		//
		LoginLayout.ColumnCount = 2;
		LoginLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
		LoginLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
		LoginLayout.Controls.Add(LoginTitle, 0, 0);
		LoginLayout.Controls.Add(UserNameLabel, 0, 1);
		LoginLayout.Controls.Add(UserNameInput, 1, 1);
		LoginLayout.Controls.Add(PasswordLabel, 0, 2);
		LoginLayout.Controls.Add(PasswordInput, 1, 2);
		LoginLayout.Controls.Add(LoginError, 0, 3);
		LoginLayout.Controls.Add(LoginActions, 0, 4);
		LoginLayout.Dock = DockStyle.Fill;
		LoginLayout.Location = new Point(0, 0);
		LoginLayout.Name = "LoginLayout";
		LoginLayout.Padding = new Padding(24);
		LoginLayout.RowCount = 5;
		LoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
		LoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
		LoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
		LoginLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
		LoginLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
		LoginLayout.Size = new Size(430, 270);
		LoginLayout.TabIndex = 0;
		//
		// LoginTitle
		//
		LoginLayout.SetColumnSpan(LoginTitle, 2);
		LoginTitle.Dock = DockStyle.Fill;
		LoginTitle.Font = new Font(LoginTitle.Font.FontFamily, 16F, FontStyle.Bold);
		LoginTitle.Location = new Point(27, 24);
		LoginTitle.Name = "LoginTitle";
		LoginTitle.Size = new Size(376, 54);
		LoginTitle.TabIndex = 0;
		LoginTitle.Text = "Contact Manager";
		LoginTitle.TextAlign = ContentAlignment.MiddleCenter;
		//
		// UserNameLabel
		//
		UserNameLabel.Anchor = AnchorStyles.Left;
		UserNameLabel.AutoSize = true;
		UserNameLabel.Location = new Point(27, 89);
		UserNameLabel.Name = "UserNameLabel";
		UserNameLabel.Size = new Size(75, 20);
		UserNameLabel.TabIndex = 1;
		UserNameLabel.Text = "Username";
		//
		// UserNameInput
		//
		UserNameInput.Anchor = AnchorStyles.Left | AnchorStyles.Right;
		UserNameInput.Location = new Point(127, 85);
		UserNameInput.Name = "UserNameInput";
		UserNameInput.Size = new Size(276, 27);
		UserNameInput.TabIndex = 2;
		//
		// PasswordLabel
		//
		PasswordLabel.Anchor = AnchorStyles.Left;
		PasswordLabel.AutoSize = true;
		PasswordLabel.Location = new Point(27, 131);
		PasswordLabel.Name = "PasswordLabel";
		PasswordLabel.Size = new Size(70, 20);
		PasswordLabel.TabIndex = 3;
		PasswordLabel.Text = "Password";
		//
		// PasswordInput
		//
		PasswordInput.Anchor = AnchorStyles.Left | AnchorStyles.Right;
		PasswordInput.Location = new Point(127, 127);
		PasswordInput.Name = "PasswordInput";
		PasswordInput.Size = new Size(276, 27);
		PasswordInput.TabIndex = 4;
		PasswordInput.UseSystemPasswordChar = true;
		//
		// LoginError
		//
		LoginLayout.SetColumnSpan(LoginError, 2);
		LoginError.Dock = DockStyle.Fill;
		LoginError.ForeColor = Color.Firebrick;
		LoginError.Location = new Point(27, 162);
		LoginError.Name = "LoginError";
		LoginError.Size = new Size(376, 42);
		LoginError.TabIndex = 5;
		LoginError.TextAlign = ContentAlignment.MiddleCenter;
		//
		// LoginActions
		//
		LoginActions.Anchor = AnchorStyles.Top;
		LoginActions.AutoSize = true;
		LoginLayout.SetColumnSpan(LoginActions, 2);
		LoginActions.Controls.Add(LoginButton);
		LoginActions.Controls.Add(CancelLoginButton);
		LoginActions.FlowDirection = FlowDirection.LeftToRight;
		LoginActions.Location = new Point(118, 207);
		LoginActions.Name = "LoginActions";
		LoginActions.Size = new Size(194, 35);
		LoginActions.TabIndex = 6;
		LoginActions.WrapContents = false;
		//
		// LoginButton
		//
		LoginButton.AutoSize = true;
		LoginButton.Location = new Point(3, 3);
		LoginButton.Name = "LoginButton";
		LoginButton.Size = new Size(94, 29);
		LoginButton.TabIndex = 0;
		LoginButton.Text = "Login";
		LoginButton.UseVisualStyleBackColor = true;
		LoginButton.Click += AttemptLogin;
		//
		// CancelLoginButton
		//
		CancelLoginButton.AutoSize = true;
		CancelLoginButton.DialogResult = DialogResult.Cancel;
		CancelLoginButton.Location = new Point(103, 3);
		CancelLoginButton.Name = "CancelLoginButton";
		CancelLoginButton.Size = new Size(88, 29);
		CancelLoginButton.TabIndex = 1;
		CancelLoginButton.Text = "Cancel";
		CancelLoginButton.UseVisualStyleBackColor = true;
		//
		// LoginForm
		//
		AcceptButton = LoginButton;
		AutoScaleDimensions = new SizeF(8F, 20F);
		AutoScaleMode = AutoScaleMode.Font;
		CancelButton = CancelLoginButton;
		ClientSize = new Size(430, 270);
		Controls.Add(LoginLayout);
		FormBorderStyle = FormBorderStyle.FixedDialog;
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "LoginForm";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Contact Manager - Login";
		LoginLayout.ResumeLayout(false);
		LoginLayout.PerformLayout();
		LoginActions.ResumeLayout(false);
		LoginActions.PerformLayout();
		ResumeLayout(false);
	}

	#endregion
}
