CREATE NONCLUSTERED INDEX [idx_cc_hearscreen_InputDate]
ON [dbo].[cc_hearscreen] (
  [inputdate] ASC
)
GO

CREATE NONCLUSTERED INDEX [idx_cc_hearscreen_ChildId]
ON [dbo].[cc_hearscreen] (
  [childid] ASC
)