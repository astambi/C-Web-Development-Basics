﻿namespace GameStore.App.Controllers
{
    using Data;
    using Data.Models;
    using SimpleMvc.Framework.Controllers;
    using System.Linq;

    public abstract class BaseController : Controller
    {
        protected const string HomePage = "/";

        public BaseController()
        {
            this.ViewModel["anonymousDisplay"] = "flex";
            this.ViewModel["userDisplay"] = "none";
            this.ViewModel["adminDisplay"] = "none";

            this.ViewModel["show-error"] = "none";
        }

        protected User Profile { get; set; }

        protected void ShowError(string error)
        {
            this.ViewModel["show-error"] = "block";
            this.ViewModel["error"] = error;
        }

        protected override void InitializeController()
        {
            base.InitializeController();

            if (this.User.IsAuthenticated)
            {
                this.ViewModel["anonymousDisplay"] = "none";

                using (var context = new GameStoreDbContext())
                {
                    this.Profile = context
                        .Users
                        .First(u => u.Email == this.User.Name);

                    if (this.Profile.IsAdmin)
                    {
                        this.ViewModel["adminDisplay"] = "flex";
                    }
                    else
                    {
                        this.ViewModel["userDisplay"] = "flex";
                    }
                }
            }
        }

        protected bool IsAdmin => this.User.IsAuthenticated && this.Profile.IsAdmin;
    }
}
