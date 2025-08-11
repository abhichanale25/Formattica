import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ConversionService } from '../../services/conversion.service';

@Component({
  selector: 'app-conversion',
  templateUrl: './conversion.component.html',
  styleUrls: ['./conversion.component.css']
})
export class ConversionComponent implements OnInit {
  conversionForm!: FormGroup;
  selectedFile: File | null = null;
  convertedFile: any;

  constructor(
    private fb: FormBuilder,
    private conversionService: ConversionService
  ) {}

  ngOnInit(): void {
    this.conversionForm = this.fb.group({
      targetFormat: ['png'] // Default selected format
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  fileConversion(): void {
    if (!this.selectedFile) {
      alert('Please select a file first.');
      return;
    }
  
    const formData = new FormData();
    formData.append('file', this.selectedFile);
  
    const targetFormat = this.conversionForm.get('targetFormat')?.value;
  
    this.conversionService.fileConversion(formData, targetFormat).subscribe({
      next: (res: any) => {
        this.convertedFile = res;
      },
      error: (err) => {
        console.error('Conversion failed', err);
        this.convertedFile = null;
      }
    });
  }
  
  downloadConvertedFile(): void {
    if (!this.convertedFile || !this.selectedFile) return;
  
    const blobUrl = URL.createObjectURL(this.convertedFile);
    const a = document.createElement('a');
    a.href = blobUrl;
  
    const originalName = this.selectedFile.name;
    const targetFormat = this.conversionForm.get('targetFormat')?.value; // e.g., 'pdf', 'docx'
  
    const baseName = originalName.substring(0, originalName.lastIndexOf('.')) || originalName;
    a.download = `converted_${baseName}.${targetFormat}`;
  
    a.click();
    URL.revokeObjectURL(blobUrl);
  }
  

  // downloadConvertedFile(): void {
  //   if (!this.convertedFile || !this.selectedFile) return;

  //   const blobUrl = URL.createObjectURL(this.convertedFile);
  //   const a = document.createElement('a');
  //   a.href = blobUrl;
  //   a.download = 'converted_' + this.selectedFile.name;
  //   a.click();
  //   URL.revokeObjectURL(blobUrl);
  // }
}
