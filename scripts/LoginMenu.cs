using Godot;
using System;
using System.Threading.Tasks;

public partial class LoginMenu : Control
{
	private Global global;
	private TextureRect error;
	private Label textoError;

	public override void _Ready()
	{
		global = GetNode<Global>("/root/Global");
		error = GetNode<TextureRect>("MensajeError");
		textoError = error?.FindChild("TextoError") as Label;

		// Asegurar que el error está oculto al inicio
		error.Visible = false;
	}

	public async void OnLoginButtonPressed()
	{
		var username = GetNode<LineEdit>("Username").Text;
		var password = GetNode<LineEdit>("Password").Text;

		// Asegurar que los campos no estén vacíos
		if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
		{
			ShowError("Username y Password cannot be empty");
			return;
		}

		// Llamar a la función de login
		await global.LoginUserAsync(username, password);

		// Si el login es exitoso, ir al menú principal
		if (!string.IsNullOrEmpty(global.Username))
		{
			global.GotoScene("res://scenes/menu/MainMenu.tscn");
		}
		else
		{
			ShowError("Login failed. Please check your credentials.");
		}
	}

	public async void OnRegisterButtonPressed()
	{
		var username = GetNode<LineEdit>("Username").Text;
		var password = GetNode<LineEdit>("Password").Text;

		// Asegurar que los campos no estén vacíos
		if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
		{
			ShowError("Username or Password cannot be empty");
			return;
		}

		// Llamar a la función de registro
		await global.RegisterUserAsync(username, password);

		// Si el registro es exitoso, ir al menú principal
		if (!string.IsNullOrEmpty(global.Username))
		{
			global.GotoScene("res://scenes/menu/MainMenu.tscn");
		}
		else
		{
			ShowError("Registration failed. Try a different username.");
		}
	}

	private void ShowError(string message)
	{
		textoError.Text = message;
		error.Visible = true;
	}

	public void OnBackButtonPressed()
	{
		// Ocultar el mensaje de error cuando se presione el botón
		error.Visible = false;
	}
}
