import { Injectable, Pipe, PipeTransform } from '@angular/core';
import { environment } from 'environments/environment.prod';

@Injectable({
  providedIn: "root"
})
@Pipe({
  name: 'image'
})
export class ImagePipe implements PipeTransform {

  transform(image: string, qualityQuery:boolean = true): any {

    if(image === "" || image === undefined || image === null || image === "---") {
      return "assets/img/placeholders/nophoto.jpg"
    }

    if(image[0] == '/') {
      return environment.mediaUrl + image + (qualityQuery ? '?width=300&quality=75' : '');
    } else {
      return image;
    }

  }

}
