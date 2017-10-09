import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpModule, JsonpModule } from '@angular/http';

import {
  AutoCompleteModule,
  ButtonModule,
  MenuItem,
  OverlayPanelModule,
  PanelMenuModule
} from 'primeng/primeng';

import { ConfigurationReader } from '../../infrastructure/services/configuration-reader.service';
import { InfrastructureModule } from '../../infrastructure/infrastructure.module';
import { MainMenuService } from './../../infrastructure/services/mainmenu.service';

import { BodyModule } from './../../common/ui/body/body.module';
import { CommonsModule } from '../../common/common.module';
import { LogoutModule } from '../../common/ui/logout/logout.module';
import { AuthenticationService } from './../../common/services/authentication.service';
import { SessionService } from './../../common/services/session.service';

import { ShoreModule } from '../../shore/shore.module';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
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
    PanelMenuModule,
    ButtonModule,
    AutoCompleteModule,
    OverlayPanelModule,
    InfrastructureModule,
    ShoreModule,
    BodyModule
  ],

  declarations: [
    AppComponent,
    LoginComponent,
    LogoutComponent,
    UnauthorizedComponent
  ],

  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: loadConfig,
      deps: [ConfigurationReader],
      multi: true
    },
    MainMenuService,
    ConfigurationReader,
    AuthenticationService,
    SessionService
  ],

  bootstrap: [AppComponent]
})

export class AppModule { }
