var bindings = {
    updateBinding : function (end1Id, end1Prop, end2Id, end2Prop)
    {
        var end1 = document.getElementById(end1Id);
        var end2 = document.getElementById(end2Id);

        end2.setAttribute(end2Prop, end1[end1Prop]);
    }
};
