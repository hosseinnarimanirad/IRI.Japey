using IRI.Jab.Common.Assets.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Presenters.General
{
    public class AboutMePresenter
    {
        private const string _githubUrl = "https://github.com/hosseinnarimanirad";
        private const string _stackoverflowUrl = "https://stackoverflow.com/users/1468295/hossein-narimani-rad?tab=profile";
        private const string _linkedinUrl = "https://www.linkedin.com/in/hosseinnarimanirad/";

        public Action RequestGoToGithub = () => { System.Diagnostics.Process.Start(_githubUrl); };

        public Action RequestGoToStackoverflow = () => { System.Diagnostics.Process.Start(_stackoverflowUrl); };

        public Action RequestGoToLinkedin = () => { System.Diagnostics.Process.Start(_linkedinUrl); };

         private void GoToWebsite(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        private RelayCommand _gotoGithubCommand;

        public RelayCommand GotoGithubCommand
        {
            get
            {
                if (_gotoGithubCommand == null)
                {
                    _gotoGithubCommand = new RelayCommand(param => this.RequestGoToGithub?.Invoke());
                }
                return _gotoGithubCommand;
            }
        }

        private RelayCommand _gotoStackoverflowCommand;

        public RelayCommand GotoStackoverflowCommand
        {
            get
            {
                if (_gotoStackoverflowCommand == null)
                {
                    _gotoStackoverflowCommand = new RelayCommand(param => this.RequestGoToStackoverflow?.Invoke());
                }
                return _gotoStackoverflowCommand;
            }
        }

        private RelayCommand _gotoLinkedinCommand;

        public RelayCommand GotoLinkedinCommand
        {
            get
            {
                if (_gotoLinkedinCommand == null)
                {
                    _gotoLinkedinCommand = new RelayCommand(param => this.RequestGoToLinkedin?.Invoke());
                }
                return _gotoLinkedinCommand;
            }
        }

    }
}
