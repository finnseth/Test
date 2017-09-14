import { forEach } from '@angular/router/src/utils/collection';
import { Directive, forwardRef, Attribute, Input } from '@angular/core';
import { Validator, AbstractControl, NG_VALIDATORS } from '@angular/forms';


@Directive({selector: '[emailvalidator][formControlName],[emailvalidator][formControl],[emailvalidator][ngModel]',
            providers: [
              { provide: NG_VALIDATORS, useExisting: forwardRef(() => EmailValidator), multi: true }
            ]
          })

  export class EmailValidator implements Validator {
  
    @Input('emailvalidator')
    set emailtype(singleormultiple: string) {
      this._emailtype = singleormultiple;
    };

    _emailtype: string;
  
    constructor(@Attribute('emailvalidator') public validateEmail) {
    }

    validate(c:AbstractControl): { [key: string]: any }{
      
      if (this._emailtype === "single") 
      {
        return this.validatesingleemailaddress(c.value);
      } else 
      {
        return this.validatemultipleemailaddress(c.value);
      }

    }

    validatesingleemailaddress(value:string): { [key: string]: any }{

      if (value === "" || value == undefined) return null;

      let regexp = new RegExp(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
      let validemailadr = regexp.test(value.trim());
      if (validemailadr){
        return null;
      } else {
        return {invalidEmail: value};
      }
    }

    validatemultipleemailaddress(values:string): { [key: string]: any }{

      if (values === "" || values == undefined) return null;

      let emails = values.split(",");
      for (let email of emails) {
        let singlevalidation = this.validatesingleemailaddress(email);
        if (singlevalidation !== null) return singlevalidation;
      }
      return null;      
    }

}
