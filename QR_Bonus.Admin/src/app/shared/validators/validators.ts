import { DatePipe } from "@angular/common";
import { AbstractControl, FormControl, FormGroup, ValidatorFn } from "@angular/forms";

export class Validator{

  private datePipe = new DatePipe('en-US');

  public password(formGroup: FormGroup) {
    const { value: password } = formGroup.get("password");
    const { value: confirmPassword } = formGroup.get("confPassword");
    formGroup.controls.confPassword?.setErrors(password === confirmPassword ? null : { passwordNotMatch: true });
    // formGroup.controls.confPassword?.setErrors(password === confirmPassword && confirmPassword !== '' ? null : { passwordNotMatch: true });
    return password === confirmPassword ? null : { passwordNotMatch: true };
  }

  public noWhitespaceValidator(length): ValidatorFn {
    return (control: AbstractControl) =>{
      const isWhitespace = (control.value || '').trim().length >= length;
      return isWhitespace ? null : { 'whitespace': true };
    };
  }

  public noWhitespaceValidator2(): ValidatorFn {
    // return (control: AbstractControl) =>{
    //   if(control.value !== null) {
    //     return control.value.includes(' ') ?  { 'whitespace': true } : null;
    //   }
    // }
    return (control: AbstractControl) =>{
      if(typeof control.value !== 'string') return null;
      if(control.value !== null) {
        if(control.value.trim() === "" || control.value.trim() === undefined || control.value.trim() === null) {
          return { 'whitespace': true };
        }
        return null
      }
    }
  }

  public isNumericValidator(): ValidatorFn {
    return (control: AbstractControl) =>{
      if(isNaN(control.value)){
        return {'isNumeric': true}
      }
      return null;
    };
  }

  public onlyNumericValidator(): ValidatorFn {
    return (control: AbstractControl) =>{
      if(isNaN(+control.value)){
        return {'onlyNumerbers': true}
      }
      return null;
    };
  }

  public noWhitespaceAtStartOrEndValidator(): ValidatorFn {
    return (control: AbstractControl) =>{
      if(control.value === '') {
        return null;
      }
      if(control.value !== null && control.value !== '') {
        let perviousValueLength = control.value.length;
        if(control.value.toString().trim().length !== perviousValueLength || control.value.trim() === '') {
          return { 'whitespace': true };
        } else {
          return null;
        }
      }
    };
  }

  public minMaxValueValidator(formGroup: FormGroup) {
    const { value: min } = formGroup.get("valueNumberFrom");
    const { value: max } = formGroup.get("valueNumberTo");
    if((min === null || min === "") || (max === null || max === "")) {
      return null;
    } else {
      return parseInt(min) <= parseInt(max) ? null : { minMaxValue: true };
    }
  }

  public notFalseValidator(): ValidatorFn {
    return (control: AbstractControl) => {
      if(control.value === false) {
        return {'isFalse': true}
      }
      return null;
    };
  }

  public phoneNumberValidator() : ValidatorFn {
    let phonePattern;
    return (control: AbstractControl) => {
      if(control.value !== null) {
        if(control.value[0] === "+") {
          phonePattern = /^(\+7[0-9]{10})$/;
        } else {
          phonePattern = /^(8[0-9]{10})$/;
        }
        let isValid = phonePattern.test(control.value);
        if(!isValid) {
          return {'invalidPhoneNumber': true}
        }
      }
      return null;
    }
  }

  public minLenghtWithoutWhitspases(min) : ValidatorFn {
    return (control: AbstractControl) => {
      if(control.value !== null) {
        let value = control.value.replace(/\s/g, '');
        if(value.toString().length < min) {
          return {'minLenghtWithoutWhitspases': true}
        }
      }
      return null;
    }
  }

  public noZeroesValidator() : ValidatorFn {
    return (control: AbstractControl) => {
      if(control.value !== null) {
        if(control.value.toString()[0] === '0') {
          return {'noZeroes': true};
        }
        if(control.value.toString()[0] === '0' && control.value.toString().includes("00")) {
          return {'noZeroes': true};
        }
        return null;
      }
    }
  }

  public dataPickerUndefinedValidator(): ValidatorFn {
    return (control: AbstractControl) => {
      if(control.value !== null && control.value === undefined) {
        return {"dataPickerInvalidValue" : true};
      }
      return null;
    }
  }

  public noWhiteSapaceValidator3(): ValidatorFn {
    return (control: AbstractControl) => {
      if(control.value !== undefined && control.value !== null && control.value[0] === " ") {
        return {'wiihespaceError' : true}
      }
      return null
    }
  }

  public whiteSpaceIncludesValidator(): ValidatorFn {
    return (control: AbstractControl) => {
      if(control.value !== undefined && control.value !== null && control.value.includes(" ")) {
        return {'includesWiihespaceError' : true}
      }
      return null
    }
  }

  public dataPickerPastDateValidator(): ValidatorFn {
    return (control: AbstractControl) => {
      if(control.value) {
        let selectedDate = (new Date(control.value.split('T')[0])).getTime();
        let nowDate = (new Date((new Date()).toISOString().split('T')[0])).getTime();
        if(selectedDate < nowDate) {
          return {'pastDate' : true};
        }
      }
      return null;
    }
  }

  //// allows only one zero
  public noZeroesValidatorVersion2() : ValidatorFn {
    return (control: AbstractControl) => {
      if(control.value !== null) {
        if(control.value.length > 1 && control.value.toString()[0] === '0') {
          return {'noZeroes': true};
        }
        if(control.value.toString()[0] === '0' && control.value.toString().includes("00")) {
          return {'noZeroes': true};
        }
        return null;
      }
    }
  }

  public minQuantValidator(formGroup: FormGroup) {
    const { value: minQuantum } = formGroup.get("minQuantum");
    const { value: packageQuantity } = formGroup.get("packageQuantity");
    if((minQuantum === null || minQuantum === "") || (packageQuantity === null || packageQuantity === "")) {
      return null;
    } else {
      return packageQuantity % minQuantum == 0 ? null : { minQuantValidator: true };
    }
  }

}
