import {DialogController} from 'aurelia-dialog';

export class ResetDialog {
   static inject = [DialogController];

   constructor(controller) {
      this.controller = controller;
      this.answer = null;

      controller.settings.centerHorizontalOnly = true;
   }
   activate(model) {
      this.resetModel = model.reset;
      this.visible = true;
   }
   
   reset() {
      this.resetModel()
      this.visible = false;
      controller.cancel()
   }

   close(){
      this.visible = false;
      controller.cancel()
   }
}