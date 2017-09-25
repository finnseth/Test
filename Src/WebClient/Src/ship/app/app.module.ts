import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpModule, JsonpModule } from '@angular/http';

import { ConfigurationReader } from '../../infrastructure/services/configuration-reader.service';
import { InfrastructureModule } from '../../infrastructure/infrastructure.module';
import { MainMenuService } from './../../infrastructure/services/mainmenu.service';

import { CommonsModule } from '../../common/common.module';
import { LogoutModule } from '../../common/ui/logout/logout.module';
import { BodyModule } from './../../common/ui/body/body.module';

import { ShipModule } from '../ship.module';

import { AppComponent } from './app.component';
import { routing } from './app.routing';

export function loadConfig(config: ConfigurationReader) {
  return () => config.load();
}

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule, JsonpModule,
    routing,
    CommonsModule,
    LogoutModule,
    BrowserAnimationsModule,
    InfrastructureModule,
    ShipModule,
    BodyModule
  ],

  declarations: [
    AppComponent
  ],

  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: loadConfig,
      deps: [ConfigurationReader],
      multi: true
    },
    ConfigurationReader,
    MainMenuService
  ],

  bootstrap: [AppComponent]
})
export class AppModule { }
