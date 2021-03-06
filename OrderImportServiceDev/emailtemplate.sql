/****** Script for SelectTopNRows command from SSMS  ******/


  if not exists(select * from emailtemplate where templatename='OrderImportError')
  insert into EmailTemplate([TemplateName], [FromAddress],  [Subject], [Message], [IsDeleted], [CreatedBy], [UpdatedBy], [CreatedDate], [UpdatedDate])
  values (
  'OrderImportError',
  'noreply@sctlogistics.com.au',
  'Online Order Error',
  '<div style="padding:5px;padding-bottom:20px;font-weight:bold;font-size:10pt;font-family:arial">   
   An error occurred when attempting to save an online customer order.    
</div>
<table style="width:100%;font-size:10pt;font-family:arial">    
    <tr>
        <td style="width:10%;padding:5px;font-weight:bold">
            Customer Account #:
        </td>
        <td style="width:90%;padding:5px">
            [CustomerAccountID]
        </td>
    </tr>
    <tr>
        <td style="padding:5px;width:10%;font-weight:bold">
            Purchase Order #:
        </td>
        <td style="padding:5px;width:90%">
            [CustomerPONum]
        </td>
    </tr>
    <tr>
        <td style="padding:5px;width:10%;font-weight:bold">
            Error:
        </td>
        <td style="padding:5px;width:90px">
            [Error]
        </td>
    </tr>
    <tr>
        <td style="padding:5px;width:10%;font-weight:bold">
            Shipping Request ID:
        </td>
        <td style="padding:5px;width:90px">
            [ShipReqID]
        </td>
    </tr>
</table>',
0, 
1,
1,
getdate(),
getdate()


  )