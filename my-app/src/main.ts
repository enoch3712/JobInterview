import {Aurelia, } from 'aurelia-framework';
import environment from '../config/environment.json';
import {PLATFORM} from 'aurelia-pal';
import {Backend, TCustomAttribute} from 'aurelia-i18n';
import XHR from 'i18next-xhr-backend';

export function configure(aurelia: Aurelia): void {
  aurelia.use
    .standardConfiguration()
    .feature(PLATFORM.moduleName('resources/index'))
    .plugin(PLATFORM.moduleName('aurelia-dialog'))
    .plugin(PLATFORM.moduleName('aurelia-validation'))
    .plugin(PLATFORM.moduleName('aurelia-i18n'), (instance) => {
      // register backend plugin
      instance.i18next.use(XHR);

      let aliases = ['t', 'i18n'];
      // add aliases for 't' attribute
      TCustomAttribute.configureAliases(aliases);

      instance.i18next.use(Backend.with(aurelia.loader));


      // adapt options to your needs (see http://i18next.com/docs/options/)
      instance.setup({
         backend: {                                  
            loadPath: '/locales/{{lng}}/{{ns}}.json',
         },
         lng : 'en',
         attributes : aliases,
         fallbackLng : 'en',
         debug : false
      });
   });

  aurelia.use.developmentLogging(environment.debug ? 'debug' : 'warn');

  if (environment.testing) {
    aurelia.use.plugin(PLATFORM.moduleName('aurelia-testing'));
  }

  aurelia.start().then(() => aurelia.setRoot(PLATFORM.moduleName('app')));
}
