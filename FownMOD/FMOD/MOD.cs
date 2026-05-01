using CommandSystem;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.FMOD
{
    public abstract class MOD
    {
        public abstract string Name { get; }
        public abstract string Author { get; }
        public abstract Version Version { get; }
        public Type ConfigType {  get; }
        public List<ICommand> Commands { get; set; }
        public virtual void OnEnabled()
        {

        }
        public virtual void OnDisable()
        {

        }
        public virtual void OnEnabledCommands()
        {
            foreach(ICommand command in Commands)
            {
                if(command is RemoteAdminCommandHandler remote)
                {
                    CommandProcessor.RemoteAdminCommandHandler.RegisterCommand((ICommand)remote);
                }
                else if(command is GameConsoleCommandHandler gameConsole)
                {
                    GameCore.Console.ConsoleCommandHandler.RegisterCommand((ICommand)gameConsole);
                }
                else if(command is ClientCommandHandler client)
                {
                    QueryProcessor.DotCommandHandler.RegisterCommand((ICommand)client);
                }
            }
        }
        public virtual void OnDisabledCommands()
        {
            foreach (ICommand command in Commands)
            {
                if (command is RemoteAdminCommandHandler remote)
                {
                    CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand((ICommand)remote);
                }
                else if (command is GameConsoleCommandHandler gameConsole)
                {
                    GameCore.Console.ConsoleCommandHandler.UnregisterCommand((ICommand)gameConsole);
                }
                else if (command is ClientCommandHandler client)
                {
                    QueryProcessor.DotCommandHandler.UnregisterCommand((ICommand)client);
                }
            }
        }
        public Object Config
        {
            get => Activator.CreateInstance(ConfigType);
        }
    }
}
