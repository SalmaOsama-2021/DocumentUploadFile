import { Component, OnInit } from '@angular/core';
import { GeneralDto } from '../../Models/generalDto';
import { ResponseHeader } from '../../Models/response-header';
import { DocumentUploadFileService } from '../../Services/DocumentUploadFileService.service';
import { ResponseMessage } from '../../Services/ResponseMessage.service';

@Component({
  selector: 'app-document-upload-file-view',
  templateUrl: './document-upload-file-view.component.html',
  styleUrls: ['./document-upload-file-view.component.scss']
})
export class DocumentUploadFileViewComponent implements OnInit {

  constructor(private _DocumentService:DocumentUploadFileService,private _responseMessage:ResponseMessage) { }

  ngOnInit(): void {
    this.getAllData();
  }
  public generalDto: GeneralDto;
  public responseResult:ResponseHeader;
  public DataList :any;
  public id:number;
  getAllData(){

    debugger;
    this.generalDto =
    {
        id:0
    }
          this._DocumentService.GetDocumentFile(this.generalDto).subscribe(res => {
              this.responseResult = res;
              if (this.responseResult.statusCode == 200) {
                this.DataList=this.responseResult.result;
                  // this._responseMessage.showsuccessmessage(this.responseResult.responseMessage);
                 
                
             
          
              } else {
                  this._responseMessage.showerrormessage(this.responseResult.responseMessage);
              }
      
          })
      
        }
        UpdateData(id: any) {
          debugger;
          this.id = id;
          window.open("DocumentUploadFileAdd?id=" + id + "", '_blank');
        }
        AddData() {
          this.id = 0;
          window.open("DocumentUploadFileAdd?id=" +   this.id + "", '_blank');
        }
        DeleteData(id:any){

          this.generalDto =
          {
              id:id
          }
          const formData = new FormData();
formData.append('requestBody', JSON.stringify( this.generalDto));
                this._DocumentService.deleteDocument(formData).subscribe(res => {
                    this.responseResult = res;
                    if (this.responseResult.statusCode == 200) {
                     
                        this._responseMessage.showsuccessmessage(this.responseResult.responseMessage);
                       this.getAllData();
                      
                   
                
                    } else {
                        this._responseMessage.showerrormessage(this.responseResult.responseMessage);
                    }
            
                })
            
              }
}
