import AssetsService from './Services/AssetsService';
import User from './DTOs/User';
import AssetDto from './DTOs/AssetDto';
import UsersService from './Services/UserService';
import { DialogService } from 'aurelia-dialog';
import Asset from './DTOs/Asset';
import {I18N} from 'aurelia-i18n';
import {ValidationRules, ValidationControllerFactory} from 'aurelia-validation';
import { ResetDialog } from './resetDialog';
import { Prompt } from './my-modal';

export class App {
  user = {} as User;
  errors = {}
  assets = [] as AssetDto[];
  users = [] as User[];
  usersSelected = false; 
  pageElements = 10;
  textSearch = null;

  static inject = [DialogService, I18N, ValidationControllerFactory];
  dialogService: any;
  i18nextService: any;
  controller: any;
  
  constructor(dialogService, i18nextService, ValidationControllerFactory) {
    this.dialogService = dialogService;

    this.i18nextService = i18nextService;
    this.i18nextService.setLocale('en')

    this.controller = ValidationControllerFactory.createForCurrentScope();

    ValidationRules
      .ensure('firstName').minLength(3).required()
      .ensure('lastName').minLength(3).required()
      .ensure('age').required().satisfies((v, o) =>
      {
          let cast = o as User; // im used to this way, sometimes is easier
          return cast.age > 18;
      })
      .ensure('address').required()
      .ensure('email').email().required()
      .on(this.user)
  }

  async bind() {
    try {
      let [assets, users] = await Promise.all([ AssetsService.getAssets(this.pageElements), 
                                                UsersService.get()]);
      this.assets = assets;
      this.users = users;

      setInterval(async () => {
        this.assets = await AssetsService.getAssets(this.pageElements);
      }, 10000);
    }catch(error)
    {
      this.handleError(error)
    }
  }

  handleError (error) {
    this.dialogService.open( {viewModel: Prompt, model: error });
  }

  async selectUsers() {
    this.usersSelected = true;

    document.getElementById("userButton").style.opacity = "1";
    document.getElementById("blockChain").style.opacity = "0.5";
  }

  async selectAssets() {
    this.usersSelected = false;

    document.getElementById("userButton").style.opacity = "0.5";
    document.getElementById("blockChain").style.opacity = "1";
  }

  async getList(pageElements) {
    this.pageElements = pageElements;

    try {
      this.assets = await AssetsService.getAssets(pageElements);
    }catch(error)
    {
      this.handleError(error)
    }
  }

  async searchangeText(textSearch) {
    this.textSearch = textSearch == "" ? null : textSearch;

    try {
      this.assets = await AssetsService.getAssets(this.pageElements, textSearch);
    }catch(error)
    {
      this.handleError(error)
    }
  }

  async deleteUser(id) {
    try {
      await UsersService.remove(id);
      this.users = await UsersService.get();
    }catch(error)
    {
      this.handleError(error)
    }
  }

  resetDialog()
  {
    //this.dialogService.open( {viewModel: ResetDialog, model: { reset: this.reset } });
  }

  reset() {
    this.user = {} as User;
  }

  async addUser() {

    let containsError = false;

    this.controller
      .validate()
      .then(result => {
        if(!result.valid) {
          containsError = true;
          result.results.forEach(element => {
            document.getElementById(element.propertyName)
              .style
              .borderColor = element.valid ? "#ced4da" : "red";

            if(!element.valid) {
              this.errors[element.propertyName] = element.message
            }else {
              this.errors[element.propertyName] = null
            }
          });
        }
      });

    if(containsError)
      return;

    try {
      let assets = this.assets.filter(a => a.added).map(a => 
        { 
          return {
            id: a.id, 
            symbol: a.symbol, 
            name: a.name
          }
        }) as Asset[];
      
      this.user.Assets = assets;
  
      await UsersService.post(this.user);
  
      this.users = await UsersService.get();

      this.selectUsers();
      
    }catch(error)
    {
      this.handleError(error)
    }
  };
}
