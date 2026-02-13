import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ImagePipe } from 'app/shared/pipes/image.pipe';
import { IAlbum, Lightbox } from 'ngx-lightbox';

@Component({
  selector: 'app-image',
  templateUrl: './image.component.html',
  styleUrls: ['./image.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ImageComponent),
      multi: true
    }
  ]
})
export class ImageComponent implements OnInit, ControlValueAccessor {
  @Input() set data(value: Array<string> | string) {
    this.generateImagesArray(value);
  }

  public images: Array<IAlbum> = [];

  @Input() height:number = 100;

  constructor(
    private _lightbox: Lightbox,
    private imagePipe: ImagePipe
  ) { }

  ngOnInit(): void {}

  onTouched = () => {};

  onChange = (value: any) => {};

  writeValue(value: Array<string> | string): void {
    this.generateImagesArray(value);
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {}

  private generateImagesArray(value: Array<string> | string) {
    if(!value || value.length === 0 || value == '' || value == undefined || value == null) return;

    if(Array.isArray(value)) {
      value.forEach(image => {
        this.images.push({
          src: this.imagePipe.transform(image, false),
          thumb: this.imagePipe.transform(image),
          downloadUrl: this.imagePipe.transform(image, false),
        })
      })
    } else {
      this.images = [{
        src: this.imagePipe.transform(value, false),
        thumb: this.imagePipe.transform(value),
        downloadUrl: this.imagePipe.transform(value, false),
      }]
    }
  }

  open(index: number): void {
    if(this.images[index].src === null || this.images[index].src === undefined || this.images[index].src === '' || this.images[index].src === '---') {
      return;
    }
    this._lightbox.open(this.images, index, {
      disableScrolling: true,
      showDownloadButton: false,
    });
  }

  close(): void {
    this._lightbox.close();
  }

}

//// https://github.com/themyth92/ngx-lightbox?tab=readme-ov-file#readme
