import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-blob-action',
  templateUrl: './blob-action.component.html',
  styleUrls: ['./blob-action.component.css']
})
export class BlobActionComponent {
  constructor(private http: HttpClient) { }  
  files: any[] = [];  
  fileToUpload!: FormData;  
  fileUpload: any;  
  fileUpoadInitiated!: boolean;  
  fileDownloadInitiated!: boolean;  
  private baseUrl = 'https://localhost:7107/api/v1'; 
  
  ngOnInit(): void {  
    this.showBlobs();  
  }  
  
  showBlobs() {  
    this.http.get<string[]>(this.baseUrl + '/listfiles').subscribe(result => {  
      this.files = result;  
      console.log(this.files)
      console.log(this.files[0])
    }, error => console.error(error));  
  }  
  
  addFile() {  
    if (!this.fileUpoadInitiated) {  
      document.getElementById('fileUpload')?.click();  
    }  
  }  
  handleFileInput(event: Event) {  
    const target = event.target as HTMLInputElement;
    const files = target.files as FileList;
    let formData: FormData = new FormData();  
    formData.append("asset", files[0], files[0].name);  
    this.fileToUpload = formData;  
    this.onUploadFiles();  
  }  
  
  onUploadFiles() {  
    if (this.fileUpoadInitiated) {  
      return;  
    }  
    this.fileUpoadInitiated = true;  
    if (this.fileToUpload == undefined) {  
      this.fileUpoadInitiated = false;  
      return false;  
    }  
    else {  
      return this.http.post(this.baseUrl + '/insertfile', this.fileToUpload) 
        .subscribe((response: any) => {  
          this.fileUpoadInitiated = false;  
          this.fileUpload = '';  
          if (response == true) {  
            this.showBlobs();  
          }  
          else {  
            alert('Error occured!');  
            this.fileUpoadInitiated = false;  
          }  
        },  
          err => console.log(err),  
        );  
  
    }  
  }  
  downloadFile(fileName: string) {  
    this.fileDownloadInitiated = true;  
    return this.http.get(this.baseUrl + '/downloadfile/' + fileName, { responseType: "blob" })  
      .subscribe((result: any) => {  
        if (result.type != 'text/plain') {  
          var blob = new Blob([result]);  
          let saveAs = require('file-saver');  
          let file = fileName;  
          saveAs(blob, file);  
          this.fileDownloadInitiated = false;  
        }  
        else {  
          this.fileDownloadInitiated = false;  
          alert('File not found in Blob!');  
        }  
      }  
      );  
  }  
  deleteFile(fileName: string) {  
    var del = confirm('Are you sure want to delete this file');  
    if (!del) return;  
    this.http.get(this.baseUrl + '/deletefile/' + fileName).subscribe(result => {  
      if (result != null) {  
        this.showBlobs();  
      }  
    }, error => console.error(error));  
  }  
}  
