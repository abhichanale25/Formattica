import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CompressorService } from '../../services/compressor.service';

@Component({
  selector: 'app-pdf-compressor',
  // standalone: true,
  // imports: [CommonModule],
  templateUrl: './pdf-compressor.component.html',
  styleUrls: ['./pdf-compressor.component.css']
})
export class PdfCompressorComponent {
  document: any;

  constructor (private compressorService: CompressorService){}

  ngOnInit(){}

  compressedBlob: Blob | null = null;
  selectedFile: File | null = null;

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }
  
  compressDocument(): void {
    if (!this.selectedFile) {
      alert('Please select a file first.');
      return;
    }
  
    const formData = new FormData();
    formData.append('file', this.selectedFile);
  
    this.compressorService.formatPDF(formData).subscribe({
      next: (blob: Blob) => {
        this.compressedBlob = blob;  // ✅ Store for download
      },
      error: (err) => {
        console.error('Compression failed', err);
        this.compressedBlob = null;
      }
    });
  }
  
  downloadCompressedPDF(): void {
    if (!this.compressedBlob || !this.selectedFile) return;
  
    const blobUrl = URL.createObjectURL(this.compressedBlob);
    const a = document.createElement('a');
    a.href = blobUrl;
  
    // Optional: Keep the original file name
    a.download = 'compressed_' + this.selectedFile.name;
  
    a.click();
    URL.revokeObjectURL(blobUrl);  // Clean up
  }

  

  // compressDocument(file: File): void {
  //   if (!file) {
  //     alert('Please select a file first.');
  //     return;
  //   }
  
  //   const formData = new FormData();
  //   formData.append('file', file);
  
  //   this.compressorService.formatPDF(formData).subscribe({
  //     next: (res: any) => {
  //       this.document = res;
  //     },
  //     error: err => {
  //       console.error('Compression failed', err);
  //     }
  //   });
  // }
  
  


}
