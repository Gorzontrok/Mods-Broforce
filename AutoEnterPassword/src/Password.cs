using RocketLib;
using System.Linq;

namespace AutoEnterPassword
{
    public class Password
    {
        public bool autoLoad = false;
        public GamePassword gamePassword = null;

        public Password(GamePassword gamePassword)
        {
            this.gamePassword = gamePassword;
        }

        public virtual void DoAction()
        {
            gamePassword.action();
        }
        public virtual string GetName()
        {
            return gamePassword.password;
        }
    }

    public class VanillaPassword : Password
    {
        public string password;
        public VanillaPassword(string password) : base(null)
        {
            this.password = password;
        }

        public override void DoAction()
        {
            char lastLetter = password.Last();
            MainMenu.instance.SetFieldValue("currentInput", password.Remove(password.Length - 1));
            MainMenu.instance.CallMethod("ProcessCharacter", lastLetter);
            Main.Log($"'{password}' loaded !");
        }

        public override string GetName()
        {
            return password;
        }
    }
}

