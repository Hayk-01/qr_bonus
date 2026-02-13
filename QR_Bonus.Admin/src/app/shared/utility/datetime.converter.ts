import { DatePipe } from "@angular/common";

export class DateTimeConverter {

    public static ConvertToUTC(year, month, day, hours = 0, minutes = 0, seconds = 0, isUTC: boolean = true): string | null {

        if(!year || !month || !day) {
            return;
        }

        month = month.toString().length === 1 ? '0' + month : month;
        day = day.toString().length === 1 ? '0' + day : day;

        let date = new Date(year, month-1, day, hours, minutes, seconds);
        const datePipe = new DatePipe('en-US');

        let result: string = '';

        if(isUTC) {
            let utcDateString: string | null = datePipe.transform(date, 'yyyy-MM-ddTHH:mm:ss', 'utc');
            result = utcDateString ? utcDateString + 'Z' : '';
        } else {
            result = datePipe.transform(date, 'yyyy-MM-ddTHH:mm:ss'); //// not UTC
        }

        return result;

    }

    public static GetDateObjectForDataPicker(value: string, isUTC: boolean = true) {
        if(value == null) return;

        if(isUTC && (value !== null && value != undefined && !value.includes('Z'))) {
            value + 'Z';
        }

        return {
          year: new Date(value).getFullYear(),
          month: new Date(value).getMonth() + 1,
          day: new Date(value).getDate(),
          hour: new Date(value).getHours(),
          minute: new Date(value).getMinutes(),
          second: new Date(value).getSeconds(),
        };
    }

}
