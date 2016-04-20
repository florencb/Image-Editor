Public Class frmImageModifier
    Dim myImage As Bitmap
    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        'OpenFIleDialog object from the OpenFIleDialog class
        'to launch an open file dialog window
        Dim open As New OpenFileDialog
        'value of Title property determinescaption of open file dialog window
        open.Title = "Image Location"

        'assigns a filter string to filter property of object
        'filter string consists of a list of filtering options to be displayed
        'each filtering option represents a file type
        'each filtering option is made up of:
        '1.) a description of the filter
        '2.) a vertical bar ( | )
        '3.) the filter pattern
        'number of filters = number of vertical bars
        open.Filter = "JPeg Image|*.jpg|All files (*.*)|*.*"

        'the following if condition serves 2 purposes:
        '1.) displays the open file dialog window
        '2.) checks if the user hits the Open button
        If open.ShowDialog() = Windows.Forms.DialogResult.OK Then
            'Filename is a string that contains the directory of the selected image and its name
            'the right of the assignment operator constructs an object from Bitmap class. 
            'The object contains the selected image.
            'The following statement assigns the new object to originalImage.
            'As a result, originalImage is an object constructed from the Bitmap class
            'originalImage contains selected image.
            myImage = New Bitmap(open.FileName, True)
            picOriginal.BorderStyle = BorderStyle.None
            'the following statement displays the selected image in the left picture box
            picOriginal.Image = myImage
            'if there is an image in the right picture box from the last conversion,
            'the following statement clears off that picture
            picConverted.Image = Nothing
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If picConverted.Image IsNot Nothing Then
            Dim save As New SaveFileDialog
            save.Title = "Save Directory"
            'the following statement assigns a filter string to the filter property
            save.Filter = "JPeg Image| *.jpg|All files (*.*)|*.*"
            'the following if condition serves two purposes:
            '1.) it displays the save dialog window
            '2.) it checks if the user hits the Save button
            If save.ShowDialog() = Windows.Forms.DialogResult.OK Then
                'the following statement uses the Save method to save the converted picture
                'to a directory specified in the save dialog window.
                picConverted.Image.Save(save.FileName)
                MessageBox.Show("The converted image has been successfully saved!")
            End If
        Else
            MessageBox.Show("There is no image to be saved.", "Error")
        End If
    End Sub

    Private Sub btnConvert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConvert.Click
        If picOriginal.Image IsNot Nothing Then
            myImage = New Bitmap(picOriginal.Image)
            Dim r, g, b, average As Integer
            Dim newColor As Color

            'the if statements only executed when user selects any of the items
            If cboFilter.SelectedItem IsNot Nothing Then
                'loops will go through every pixel of height and width
                For y = 0 To myImage.Height - 1
                    For x = 0 To myImage.Width - 1
                        'get pixel returns color of specified pixel
                        Dim pixelColor As Color = myImage.GetPixel(x, y)
                        'R property contains a byte value that represents red
                        r = CInt(pixelColor.R)
                        'G represents green
                        g = CInt(pixelColor.G)
                        'B represents blue
                        b = CInt(pixelColor.B)

                        Select Case cboFilter.SelectedItem.ToString
                            Case "   Monochrome"
                                average = CInt((r + g + b) / 3)
                                If (average > 128) Then
                                    r = 255
                                    g = 255
                                    b = 255
                                Else
                                    r = 0
                                    g = 0
                                    b = 0
                                End If

                                'constructs another color object that stored new color info of pixel
                                newColor = Color.FromArgb(r, g, b)
                            Case "   Gray - Averaging"
                                average = CInt((r + g + b) / 3)
                                'construct another color object that stores new info for pixel
                                newColor = Color.FromArgb(average, average, average)
                        End Select
                        'set pixel method uses new color to convert it
                        myImage.SetPixel(x, y, newColor)
                    Next
                Next
                'the following statement displays converted image in right pic box
                picConverted.Image = myImage
                picConverted.BorderStyle = BorderStyle.None
            Else
                MessageBox.Show("Please select a filter to work with.", "Error")
            End If
            MessageBox.Show("Please select a picture to convert.", "Error")
        End If
    End Sub
End Class
