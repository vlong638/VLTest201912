
CREATE NONCLUSTERED INDEX idx_cc_cc_generalinfo_status
ON [dbo].[cc_generalinfo] ([status])
INCLUDE ([createUnit],[birthDay],[createDate])
GO


CREATE NONCLUSTERED INDEX [idx_cc_physicalexam_new_inputDate ]
ON [dbo].[cc_physicalexam_new] (
  [inputDate] ASC
)
GO

CREATE NONCLUSTERED INDEX [idx_cc_physicalexam_new_followAgencyCode]
ON [dbo].[cc_physicalexam_new] (
  [followAgencyCode] ASC
)
GO


CREATE NONCLUSTERED INDEX [idx_cc_physicalexam_new_ChildId]
ON [cc_physicalexam_new] (
  [childId] ASC
)