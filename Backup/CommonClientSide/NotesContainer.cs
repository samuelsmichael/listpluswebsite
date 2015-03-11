using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CommonClientSide {
    public interface NotesContainer {
        void UpdateBeforeNotesSubmit();
        void CauseNotesSubmitButtonToHappen();
    }
}
