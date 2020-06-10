using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SDC.Schema
{
    public interface IDisplayedType
    {         
        BlobType AddBlob(int insertPosition = -1);
        LinkType AddLink(int insertPosition = -1);
        ContactType AddContact(int insertPosition = -1);
        CodingType AddCodedValue(int insertPosition = -1);

        BlobType AddBlobI(int insertPosition = -1)
        {
            var dtParent = this as DisplayedType;
            var blob = new BlobType(dtParent);
            if (dtParent.BlobContent == null) dtParent.BlobContent = new List<BlobType>();
            var count = dtParent.BlobContent.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            dtParent.BlobContent.Insert(insertPosition, blob);
            return blob;
        }
        LinkType AddLinkI(int insertPosition = -1)
        {
            var dtParent = this as DisplayedType;
            var link = new LinkType(dtParent);

            if (dtParent.Link == null) dtParent.Link = new List<LinkType>();
            var count = dtParent.Link.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            dtParent.Link.Insert(insertPosition, link);
            link.order = link.ObjectID;

            var rtf = new RichTextType(link);
            link.LinkText = rtf;

            return link;
        }
        ContactType AddContactI(int insertPosition = -1)
        {
            var dtParent = this as DisplayedType;
            if (dtParent.Contact == null) dtParent.Contact = new List<ContactType>();
            var ct = new ContactType(dtParent);
            var count = dtParent.Contact.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            dtParent.Contact.Insert(insertPosition, ct);
            return ct;
        }
        CodingType AddCodedValueI(int insertPosition = -1)
        {
            var dtParent = this as DisplayedType;
            if (dtParent.CodedValue == null) dtParent.CodedValue = new List<CodingType>();
            var ct = new CodingType(dtParent);
            var count = dtParent.CodedValue.Count;
            if (insertPosition < 0 || insertPosition > count) insertPosition = count;
            dtParent.CodedValue.Insert(insertPosition, ct);
            return ct;
        }



        PredGuardType AddActivateIf();
        PredGuardType AddDeActivateIf();
        EventType AddOnEnter();
        OnEventType AddOnEvent();
        EventType AddOnExit();



        virtual internal PredGuardType AddActivateIfI()
        {
            var dt = this as DisplayedType;
            var pg = new PredGuardType(dt, "ActivateIf", "acif");
            dt.ActivateIf = pg;
            return pg; }
        virtual internal PredGuardType AddDeActivateIfI()
        {
            var dt = this as DisplayedType;
            var pg = new PredGuardType(dt, "DeActivateIf", "deif");
            dt.DeActivateIf = pg;
            return pg;
        }
        virtual internal EventType AddOnEnterI()
        {
            var dt = this as DisplayedType;
            var ev = new EventType(dt, "OnEnter", "onen");
            dt.OnEnter.Add(ev);
            return ev;
        }
        virtual internal OnEventType AddOnEventI()
        {
            var dt = this as DisplayedType;
            var oe = new OnEventType(dt, "OnEvent", "onev");
            dt.OnEvent.Add(oe);
            return oe;
        }
        virtual internal EventType AddOnExitI()
        {
            var dt = this as DisplayedType;
            var oe = new EventType(dt, "OnExit", "onex");
            dt.OnEnter.Add(oe);
            return oe;
        }

        bool MoveEvent(EventType ev, List<EventType> targetList = null, int index = -1);
        virtual internal bool MoveEventI(EventType ev, List<EventType> targetList = null, int index = -1)
        { 
            throw new NotImplementedException(); 
        }


    //Convert to LI (if inside a List)
    //Convert to LIR (if inside a List)
    //Move (to ChildItems or List node, index for postition)
    //Can Move
    //under ChildItems - Convert to: QS/QM/QR, S
    //under List: Convert to LI/LIR

}
}
