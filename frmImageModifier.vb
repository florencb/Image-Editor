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
            Dim r, g, b, average, averageR, averageB, averageG As Integer
            Dim newColor As Color

            'the if statements only executed when user selects any of the items
            If cboFilter.SelectedItem IsNot Nothing Then

                Select Case cboFilter.SelectedItem.ToString

                    Case "   Monochrome"
                        Me.ConvertImageMonochrome(myImage, newColor, r, g, b, average)

                    Case "   Sepia"
                        Me.ConvertImageSepia(myImage, newColor, r, g, b, average, averageR, averageG, averageB)

                    Case "   Gray - Averaging"
                        Me.ConvertImageGrayAveraging(myImage, newColor, r, g, b, average)

                    Case "   Luma"
                        Me.ConvertImageLuma(myImage, newColor, r, g, b, average)

                    Case "   Desaturation"
                        Me.ConvertImageDesaturation(myImage, newColor, r, g, b, average)

                    Case "   DecompositionMax"
                        Me.ConvertImageDecompositionMin(myImage, newColor, r, g, b, average)

                    Case "   DecompositionMin"
                        Me.ConvertImageDecompositionMin(myImage, newColor, r, g, b, average)

                    Case "   SingleChannelRed"
                        Me.ConvertImageSingleRed(myImage, newColor, r, g, b, average)

                    Case "   SingleChannelGreen"
                        Me.ConvertImageSingleGreen(myImage, newColor, r, g, b, average)

                    Case "   SingleChannelBlue"
                        Me.ConvertImageSingleBlue(myImage, newColor, r, g, b, average)

                End Select  
                'the following statement displays converted image in right pic box
                picConverted.Image = myImage
                picConverted.BorderStyle = BorderStyle.None
            Else
                MessageBox.Show("Please select a filter to work with.", "Error")
            End If
            MessageBox.Show("Please select a picture to convert.", "Error")
        End If
    End Sub

    Private Sub ConvertImageMonochrome(ByRef myImage As Bitmap, myNewColor As Color, myR As Integer, myG As Integer, myB As Integer, myAverage As Integer)
        'loops will go through every pixel of height and width
        For y = 0 To myImage.Height - 1
            For x = 0 To myImage.Width - 1
                'get pixel returns color of specified pixel
                Dim pixelColor As Color = myImage.GetPixel(x, y)
                'R property contains a byte value that represents red
                myR = CInt(pixelColor.R)
                'G represents green
                myG = CInt(pixelColor.G)
                'B represents blue
                myB = CInt(pixelColor.B)

                myAverage = CInt((myR + myG + myB) / 3)
                If (myAverage > 128) Then
                    myR = 255
                    myG = 255
                    myB = 255
                Else
                    myR = 0
                    myG = 0
                    myB = 0
                End If
                'constructs another color object that stored new color info of pixel
                myNewColor = Color.FromArgb(myR, myG, myB)
                'set pixel method uses new color to convert it
                myImage.SetPixel(x, y, myNewColor)
            Next
        Next
        'the following statement displays converted image in right pic box
        picConverted.Image = myImage
        picConverted.BorderStyle = BorderStyle.None
    End Sub
    Private Sub ConvertImageSepia(ByRef myImage As Bitmap, myNewColor As Color, myR As Integer, myG As Integer, myB As Integer,
                         myAverageR As Integer, myAverageG As Integer, myAverageB As Integer, myAverage As Integer)
        For y = 0 To myImage.Height - 1
            For x = 0 To myImage.Width - 1
                Dim pixelColor As Color = myImage.GetPixel(x, y)
                myR = CInt(pixelColor.R)
                myG = CInt(pixelColor.G)
                myB = CInt(pixelColor.B)

                myAverageR = CInt((myR * 0.393) + (myG * 0.769) + (myB * 0.189))
                myAverageG = CInt((myR * 0.349) + (myG * 0.686) + (myB * 0.168))
                myAverageB = CInt((myR * 0.272) + (myG * 0.534) + (myB * 0.131))
                'if average is greater then 255 set to 255
                If (myAverageR > 255) Then
                    myAverageR = 255
                    If (myAverageG > 255) Then
                        myAverageG = 255
                        If (myAverageB > 255) Then
                            myAverageB = 255
                        End If
                    End If
                End If
                myNewColor = Color.FromArgb(myAverageR, myAverageG, myAverageB)
                myImage.SetPixel(x, y, myNewColor)
            Next
        Next
        picConverted.Image = myImage
        picConverted.BorderStyle = BorderStyle.None
    End Sub

    Private Sub ConvertImageGrayAveraging(ByRef myImage As Bitmap, myNewColor As Color, myR As Integer, myG As Integer, myB As Integer, myAverage As Integer)
        For y = 0 To myImage.Height - 1
            For x = 0 To myImage.Width - 1
                Dim pixelColor As Color = myImage.GetPixel(x, y)
                myR = CInt(pixelColor.R)
                myG = CInt(pixelColor.G)
                myB = CInt(pixelColor.B)

                myAverage = CInt((myR + myG + myB) / 3)
                myNewColor = Color.FromArgb(myAverage, myAverage, myAverage)
                myImage.SetPixel(x, y, myNewColor)
            Next
        Next
        picConverted.Image = myImage
        picConverted.BorderStyle = BorderStyle.None
    End Sub

    Private Sub ConvertImageLuma(ByRef myImage As Bitmap, myNewColor As Color, myR As Integer, myG As Integer, myB As Integer, myAverage As Integer)
        For y = 0 To myImage.Height - 1
            For x = 0 To myImage.Width - 1
                Dim pixelColor As Color = myImage.GetPixel(x, y)
                myR = CInt(pixelColor.R)
                myG = CInt(pixelColor.G)
                myB = CInt(pixelColor.B)

                myAverage = CInt((myR * 0.2126) + (myG * 0.7152) + (myB * 0.0722))
                myNewColor = Color.FromArgb(myAverage, myAverage, myAverage)
                myImage.SetPixel(x, y, myNewColor)
            Next
        Next
        picConverted.Image = myImage
        picConverted.BorderStyle = BorderStyle.None
    End Sub

    Private Sub ConvertImageDesaturation(ByRef myImage As Bitmap, myNewColor As Color, myR As Integer, myG As Integer, myB As Integer, myAverage As Integer)
        For y = 0 To myImage.Height - 1
            For x = 0 To myImage.Width - 1
                Dim pixelColor As Color = myImage.GetPixel(x, y)
                myR = CInt(pixelColor.R)
                myG = CInt(pixelColor.G)
                myB = CInt(pixelColor.B)

                myAverage = CInt(Math.Max(myR, Math.Max(myG, myB)) + Math.Min(myR, Math.Max(myG, myB)) / 2)
                'if average is greater then 255 set to 255
                If (myAverage > 255) Then
                    myAverage = 255
                End If
                myNewColor = Color.FromArgb(myAverage, myAverage, myAverage)
                myImage.SetPixel(x, y, myNewColor)
            Next
        Next
        picConverted.Image = myImage
        picConverted.BorderStyle = BorderStyle.None
    End Sub

    Private Sub ConvertImageDecompositionMin(ByRef myImage As Bitmap, myNewColor As Color, myR As Integer, myG As Integer, myB As Integer, myAverage As Integer)
        For y = 0 To myImage.Height - 1
            For x = 0 To myImage.Width - 1
                Dim pixelColor As Color = myImage.GetPixel(x, y)
                myR = CInt(pixelColor.R)
                myG = CInt(pixelColor.G)
                myB = CInt(pixelColor.B)

                myAverage = CInt(Math.Max(myR, Math.Max(myG, myB)))
                'if average is greater then 255 set to 255
                If (myAverage > 255) Then
                    myAverage = 255
                End If
                myNewColor = Color.FromArgb(myAverage, myAverage, myAverage)
                myImage.SetPixel(x, y, myNewColor)
            Next
        Next
        picConverted.Image = myImage
        picConverted.BorderStyle = BorderStyle.None
    End Sub

    Private Sub ConvertImageDecompositionMax(ByRef myImage As Bitmap, myNewColor As Color, myR As Integer, myG As Integer, myB As Integer, myAverage As Integer)
        For y = 0 To myImage.Height - 1
            For x = 0 To myImage.Width - 1
                Dim pixelColor As Color = myImage.GetPixel(x, y)
                myR = CInt(pixelColor.R)
                myG = CInt(pixelColor.G)
                myB = CInt(pixelColor.B)

                myAverage = CInt(Math.Min(myR, Math.Min(myG, myB)))
                myNewColor = Color.FromArgb(myAverage, myAverage, myAverage)
                myImage.SetPixel(x, y, myNewColor)
            Next
        Next
        picConverted.Image = myImage
        picConverted.BorderStyle = BorderStyle.None
    End Sub

    Private Sub ConvertImageSingleRed(ByRef myImage As Bitmap, myNewColor As Color, myR As Integer, myG As Integer, myB As Integer, myAverage As Integer)
        For y = 0 To myImage.Height - 1
            For x = 0 To myImage.Width - 1
                Dim pixelColor As Color = myImage.GetPixel(x, y)
                myR = CInt(pixelColor.R)
                myG = CInt(pixelColor.G)
                myB = CInt(pixelColor.B)

                myAverage = CInt(myR)
                myNewColor = Color.FromArgb(myAverage, myAverage, myAverage)
                myImage.SetPixel(x, y, myNewColor)
            Next
        Next
        picConverted.Image = myImage
        picConverted.BorderStyle = BorderStyle.None
    End Sub
    Private Sub ConvertImageSingleBlue(ByRef myImage As Bitmap, myNewColor As Color, myR As Integer, myG As Integer, myB As Integer, myAverage As Integer)
        For y = 0 To myImage.Height - 1
            For x = 0 To myImage.Width - 1
                Dim pixelColor As Color = myImage.GetPixel(x, y)
                myR = CInt(pixelColor.R)
                myG = CInt(pixelColor.G)
                myB = CInt(pixelColor.B)

                myAverage = CInt(myB)
                myNewColor = Color.FromArgb(myAverage, myAverage, myAverage)
                myImage.SetPixel(x, y, myNewColor)
            Next
        Next
        picConverted.Image = myImage
        picConverted.BorderStyle = BorderStyle.None
    End Sub
    Private Sub ConvertImageSingleGreen(ByRef myImage As Bitmap, myNewColor As Color, myR As Integer, myG As Integer, myB As Integer, myAverage As Integer)
        For y = 0 To myImage.Height - 1
            For x = 0 To myImage.Width - 1
                Dim pixelColor As Color = myImage.GetPixel(x, y)
                myR = CInt(pixelColor.R)
                myG = CInt(pixelColor.G)
                myB = CInt(pixelColor.B)

                myAverage = CInt(myG)
                myNewColor = Color.FromArgb(myAverage, myAverage, myAverage)
                myImage.SetPixel(x, y, myNewColor)
            Next
        Next
        picConverted.Image = myImage
        picConverted.BorderStyle = BorderStyle.None
    End Sub
End Class
